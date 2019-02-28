using System;
using System.Collections.Generic;

namespace Quanta.DataAccess.Models
{
    public class User
    {
        public User()
        {
            UserDevices = new HashSet<UserDevice>();
            DeviceAccess = new HashSet<DeviceAccess>();
        }

        public int Id { get; set; }

        public Guid UserIdentity { get; set; }

        public ICollection<UserDevice> UserDevices { get; set; }

        public ICollection<DeviceAccess> DeviceAccess { get; set; }
    }
}