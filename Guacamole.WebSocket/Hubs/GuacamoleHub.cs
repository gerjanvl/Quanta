using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guacamole.Client;
using Guacamole.Data;
using Guacamole.Domain.Services;
using Guacamole.WebSocket.OData;
using Guacamole.WebSocket.OData.Models;
using Guacamole.WebSocket.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Guacamole.WebSocket.Hubs
{
    [Authorize]
    public class GuacamoleHub : Hub
    {
        private readonly GuacamoleClientConnectionManager _clientConnectionManager;
        private readonly UserService _userService;

        public GuacamoleHub(GuacamoleClientConnectionManager clientConnectionManager, UserService userService)
        {
            _clientConnectionManager = clientConnectionManager;
            _userService = userService;
        }

        public async Task Connect(ConnectViewModel connectViewModel)
        {
            try
            {
                var user = _userService.GetByAdIdentifier<User>(Context.User.GetUserAdId());

                if (user == null) Context.Abort();

                var deviceConnectionString = _userService.GetUserDeviceConnectionString(user.Id, connectViewModel.DeviceId);

                if (!string.IsNullOrWhiteSpace(deviceConnectionString))
                {
                    var deviceConfiguration = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(deviceConnectionString);
                    await _clientConnectionManager.CreateNew(Context.ConnectionId,
                        protocol: deviceConfiguration["protocol"],
                        args: deviceConfiguration);
                }

            }
            catch (Exception)
            {
                Context.Abort();
                throw;
            }
        }

        public async Task WriteInstruction(GuacamoleInstruction instruction)
        {
            try
            {
                var guacamoleClient = _clientConnectionManager.GetClient(Context.ConnectionId);

                if (guacamoleClient != null)
                    await guacamoleClient.WriteInstruction(instruction.OpCode, instruction.Args.ToArray());
            }
            catch (Exception)
            {
                Context.Abort();
                throw;
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _clientConnectionManager.Remove(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}