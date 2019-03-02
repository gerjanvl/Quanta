using System;
using System.Collections.Generic;

namespace Quanta.DataAccess.Entities
{
    public class User
    {
        public User()
        {
            UserDevices = new HashSet<UserDevice>();
            Sessions = new HashSet<Session>();
        }

        public Guid Id { get; set; }

        public ICollection<UserDevice> UserDevices { get; set; }

        public ICollection<Session> Sessions { get; set; }
    }
}