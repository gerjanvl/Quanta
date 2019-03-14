using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Guacamole.Client;
using Guacamole.Client.Extensions;
using Guacamole.Client.Protocol;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quanta.Infrastructure.Services;
#pragma warning disable 4014

namespace Quanta.Infrastructure.Guacamole
{
    public class GuacamoleClientConnectionManager<THub> where THub : Hub
    {
        private readonly IHubContext<THub> _hubContext;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        private readonly GuacamoleClientManager _clientManager;
        private readonly IPEndPoint _guacamoleServerAddress;

        private static readonly string[] _blackListedInstructions = { "disconnect" , "select", "connect" };

        public GuacamoleClientConnectionManager(
            IHubContext<THub> hubContext, 
            IConfiguration configuration, 
            GuacamoleClientManager clientManager,
            IServiceProvider serviceProvider)
        {
            _hubContext = hubContext;
            _configuration = configuration;
            _clientManager = clientManager;
            _serviceProvider = serviceProvider;
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
            using (var scope = _serviceProvider.CreateScope())
            {
                var sessionService = (ISessionService)scope.ServiceProvider.GetService(typeof(ISessionService));
                var guacamoleClient = new GuacamoleClient(_guacamoleServerAddress);

                var connectionId = await guacamoleClient.Connect(protocol, width, height, args);
                sessionService.StartNew(connectionId, userId, deviceId);

                _clientManager.Add(hubConnectionId, guacamoleClient);

                var client = _hubContext.Clients.Client(hubConnectionId);
                await client.SendCoreAsync("connected", new object[] {connectionId});
            }

            ReadInstructions(hubConnectionId);
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
                    var instruction = await guacamoleClient.ReadInstruction()
                        .ThrowTimeoutAfter(TimeSpan.FromMinutes(1));

                    if (IsBlackListedInstruction(instruction)) break;

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

        private bool IsBlackListedInstruction(GuacamoleInstruction instruction)
        {
            var instructionOpCode = instruction.OpCode.ToLower();
            
            var isTextRegex = new Regex(@"^[a-zA-Z]*$");

            return !isTextRegex.IsMatch(instructionOpCode) ||_blackListedInstructions.Contains(instructionOpCode);
        }

        public void Remove(string hubConnectionId)
        {
            if(string.IsNullOrEmpty(hubConnectionId)) return;

            var client = _clientManager.Get(hubConnectionId);

            if (client?.ConnectionId != null)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var sessionService = (ISessionService)scope.ServiceProvider.GetService(typeof(ISessionService));
                    sessionService.Finish(client.ConnectionId);
                }
            }
            
            _clientManager.Remove(hubConnectionId);
        }
    }
}