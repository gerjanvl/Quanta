using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Guacamole.Client;
using Guacamole.Client.Common;
using Guacamole.Client.Extensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Quanta.Infrastructure.Guacamole;
using Quanta.Infrastructure.Services;

namespace Quanta.WebApi.Hubs
{
    public class GuacamoleClientConnectionManager
    {
        private readonly IHubContext<GuacamoleHub> _hubContext;
        private readonly IConfiguration _configuration;
        private readonly GuacamoleClientManager _clientManager;
        private readonly ISessionService _sessionService;
        private readonly IPEndPoint _guacamoleServerAddress;

        public GuacamoleClientConnectionManager(
            IHubContext<GuacamoleHub> hubContext, 
            IConfiguration configuration, 
            GuacamoleClientManager clientManager,
            ISessionService sessionService)
        {
            _hubContext = hubContext;
            _configuration = configuration;
            _clientManager = clientManager;
            _sessionService = sessionService;
            _guacamoleServerAddress = GetServerAddress();
        }

        private IPEndPoint GetServerAddress()
        {
            var ipAddress = Dns.GetHostAddresses(_configuration["Quanta:Server:Hostname"]);
            var port = _configuration.GetValue<int>("Quanta:Server:Port");

            return new IPEndPoint(ipAddress.First(), port);
        }

        public async Task CreateNew(string hubConnectionId, Guid userId, Guid deviceId, string protocol, int width, int height, Dictionary<string, object> args)
        {
            var guacamoleClient = new GuacamoleClient(_guacamoleServerAddress);

            var connectionId = await guacamoleClient.Connect(protocol, width, height, args);
            _sessionService.StartNew(connectionId, userId, deviceId);

            _clientManager.Add(hubConnectionId, guacamoleClient);

            var client = _hubContext.Clients.Client(hubConnectionId);
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

            var blockedInstructions = new List<string>()
            {
                "disconnect" , "select", "connect"
            };

            try
            {
                var client = _hubContext.Clients.Client(hubConnectionId);

                while (guacamoleClient.Connected)
                {
                    var instruction = await guacamoleClient.ReadInstruction().ThrowTimeoutAfter(TimeSpan.FromMinutes(1));

                    var instructionOpCode = instruction.OpCode.ToLower().Trim();
                    if (blockedInstructions.Contains(instructionOpCode)) break;

                    await client.SendAsync("instruction", instruction);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Remove(hubConnectionId);
            }
        }

        public void Remove(string hubConnectionId)
        {
            if(string.IsNullOrEmpty(hubConnectionId)) return;

            var client = _clientManager.Get(hubConnectionId);

            if (client?.ConnectionId != null)
            {
                _sessionService.Finish(client.ConnectionId);
            }
            
            _clientManager.Remove(hubConnectionId);
        }
    }
}