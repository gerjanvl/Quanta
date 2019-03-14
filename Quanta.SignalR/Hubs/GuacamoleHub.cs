using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guacamole.Client.Protocol;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Quanta.Extensions.OData;
using Quanta.Infrastructure.Guacamole;
using Quanta.Infrastructure.Services;
using Quanta.SignalR.ViewModels;

namespace Quanta.SignalR.Hubs
{
    [Authorize]
    public class GuacamoleHub : Hub
    {
        private readonly GuacamoleClientConnectionManager<GuacamoleHub> _clientConnectionManager;
        private readonly IServiceProvider _serviceProvider;
        private IUserService _userService;

        public GuacamoleHub(GuacamoleClientConnectionManager<GuacamoleHub> clientConnectionManager, IServiceProvider serviceProvider)
        {
            _clientConnectionManager = clientConnectionManager;
            _serviceProvider = serviceProvider;
        }

        public async Task Connect(ConnectViewModel connectViewModel)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    _userService = (IUserService)scope.ServiceProvider.GetService(typeof(IUserService));
                    var userId = Context.User.GetUserAdId();

                    if (!_userService.UserExists(userId)) Context.Abort();

                    var deviceConnectionString =
                        _userService.GetUserDeviceConnectionString(userId, connectViewModel.DeviceId);

                    if (!string.IsNullOrWhiteSpace(deviceConnectionString))
                    {
                        var deviceConfiguration =
                            JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(deviceConnectionString);

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