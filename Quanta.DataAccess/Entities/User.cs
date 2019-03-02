using System;
using System.Collections.Generic;

namespace Quanta.DataAccess.Entities
{
    public class User
    {
        public User()
        {
            UserDevices = new HashSet<UserDevice>();
            DeviceAccess = new HashSet<DeviceAccess>();
        }

        public Guid Id { get; set; }

        public ICollection<UserDevice> UserDevices { get; set; }

        public ICollection<DeviceAccess> DeviceAccess { get; set; }
    }
}