using System;
using Newtonsoft.Json;

namespace Quanta.SignalR.ViewModels
{
    public class ConnectViewModel
    {
        public Guid DeviceId { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
