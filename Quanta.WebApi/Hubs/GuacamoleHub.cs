using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guacamole.Client;
using Guacamole.Client.Protocol;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Quanta.Infrastructure.Guacamole;
using Quanta.Infrastructure.Services;
using Quanta.WebApi.Extensions.OData;
using Quanta.WebApi.OData;
using Quanta.WebApi.OData.Models;
using Quanta.WebApi.ViewModels;

namespace Quanta.WebApi.Hubs
{
    [Authorize]
    public class GuacamoleHub : Hub
    {
        private readonly GuacamoleClientConnectionManager _clientConnectionManager;
        private readonly IUserService _userService;

        public GuacamoleHub(GuacamoleClientConnectionManager clientConnectionManager, IUserService userService)
        {
            _clientConnectionManager = clientConnectionManager;
            _userService = userService;
        }

        public async Task Connect(ConnectViewModel connectViewModel)
        {
            try
            {
                var userId = Context.User.GetUserAdId();

                if (!_userService.UserExists(userId)) Context.Abort();

                var deviceConnectionString = _userService.GetUserDeviceConnectionString(userId, connectViewModel.DeviceId);

                if (!string.IsNullOrWhiteSpace(deviceConnectionString))
                {
                    var deviceConfiguration = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(deviceConnectionString);

                    await _clientConnectionManager.CreateNew(
                        hubConnectionId: Context.ConnectionId,
                        deviceId: connectViewModel.DeviceId,
                        userId: userId,
                        protocol: deviceConfiguration["protocol"],
                        width: connectViewModel.Width,
                        height: connectViewModel.Height,
                        args: deviceConfiguration
                    );
                }

            }
            catch (Exception)
            {
                _clientConnectionManager.Remove(Context.ConnectionId);
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
                _clientConnectionManager.Remove(Context.ConnectionId);
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