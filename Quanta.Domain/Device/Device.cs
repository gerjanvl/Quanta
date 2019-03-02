using System;
using System.Collections.Generic;

namespace Quanta.Domain.Device
{
    public class Device : ITrackableEntity
    {
        public Device()
        {
            Sessions = new HashSet<Session.Session>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public string OperatingSystem { get; set; }

        public bool Enabled { get; set; }

        public ICollection<Session.Session> Sessions { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastModified { get; set; }
    }
}
