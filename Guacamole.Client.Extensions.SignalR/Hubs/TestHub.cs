using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guacamole.Client;
using Guacamole.Data;
using Guacamole.WebSocket.ViewModels;
using Microsoft.AspNetCore.SignalR;

namespace Guacamole.WebSocket
{
    public class TestHub : Hub
    {
        private readonly GuacamoleClientConnectionManager _clientConnectionManager;
        private readonly GuacamoleContext _context;

        public TestHub(GuacamoleClientConnectionManager clientConnectionManager, GuacamoleContext context)
        {
            _clientConnectionManager = clientConnectionManager;
            _context = context;
        }

        public async Task Connect(ConnectViewModel connectView)
        {
            // TODO lookup machine and check if its connected to the user
            await _clientConnectionManager.CreateNew(Context.ConnectionId,
                protocol: "rdp",
                args: new Dictionary<string, object>
                {
                    {"hostname", "DESKTOP-21PNDNK"},
                    {"port", 3389},
                    {"username", "admin"},
                    {"password", "roadrunner"},
                    {"ignore-cert", true},
                    {"security", "nla"}
                });
        }

        public async Task WriteInstruction(GuacamoleInstruction instruction)
        {
            var guacamoleClient = _clientConnectionManager.GetClient(Context.ConnectionId);

            if (guacamoleClient != null)
                await guacamoleClient.WriteInstruction(instruction.OpCode, instruction.Args.ToArray());
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _clientConnectionManager.Remove(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}