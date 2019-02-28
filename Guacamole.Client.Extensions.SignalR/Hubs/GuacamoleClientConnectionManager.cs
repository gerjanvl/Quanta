using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Guacamole.Client;
using Guacamole.Client.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace Guacamole.WebSocket
{
    public class GuacamoleClientConnectionManager
    {
        private readonly IHubContext<TestHub> _hubContext;
        private readonly IConfiguration _configuration;
        private readonly GuacamoleClientManager _clientManager;
        private readonly IPEndPoint _guacamoleServerAddress;

        public GuacamoleClientConnectionManager(IHubContext<TestHub> hubContext, IConfiguration configuration, GuacamoleClientManager clientManager)
        {
            _hubContext = hubContext;
            _configuration = configuration;
            _clientManager = clientManager;
            _guacamoleServerAddress = GetServerAddress();
        }

        private IPEndPoint GetServerAddress()
        {
            var ipAddress = Dns.GetHostAddresses(_configuration["Guacamole:Server:Hostname"]);
            var port = _configuration.GetValue<int>("Guacamole:Server:Port");
            return new IPEndPoint(ipAddress.First(), port);
        }

        public async Task CreateNew(string hubConnectionId, string protocol, Dictionary<string, object> args)
        {
            var guacamoleClient = new GuacamoleClient(_guacamoleServerAddress);

            var client = _hubContext.Clients.Client(hubConnectionId);

            var connectionId = await guacamoleClient.Connect(
                protocol: protocol,
                args: args);

            _clientManager.Add(hubConnectionId, guacamoleClient);

            await client.SendCoreAsync("connected", new object[] { connectionId });

#pragma warning disable 4014
            ReadInstructions(hubConnectionId);
#pragma warning restore 4014
        }

        public GuacamoleClient GetClient(string hubConnectionId)
        {
            return _clientManager.Get(hubConnectionId);
        }

        private async Task ReadInstructions(string hubConnectionId)
        {
            var guacamoleClient = _clientManager.Get(hubConnectionId);

            try
            {
                var client = _hubContext.Clients.Client(hubConnectionId);

                while (guacamoleClient.Connected)
                {
                    var instruction = await guacamoleClient.ReadInstruction().TimeoutAfter(TimeSpan.FromMinutes(1));

                    if (instruction.OpCode.ToLower().Trim() == "disconnect") break;
                    if (instruction.OpCode.ToLower().Trim() == "select") break;
                    if (instruction.OpCode.ToLower().Trim() == "connect") break;

                    await client.SendAsync("instruction", instruction);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                _clientManager.Remove(hubConnectionId);
            }
        }

        public void Remove(string hubConnectionId)
        {
            _clientManager.Remove(hubConnectionId);
        }
    }
}