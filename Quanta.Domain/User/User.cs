using System;
using System.Collections.Generic;

namespace Quanta.Domain.User
{
    public class User
    {
        public User()
        {
            UserDevices = new HashSet<UserDevice.UserDevice>();
            Sessions = new HashSet<Session.Session>();
        }

        public Guid Id { get; set; }

        public ICollection<UserDevice.UserDevice> UserDevices { get; set; }

        public ICollection<Session.Session> Sessions { get; set; }
    }
}