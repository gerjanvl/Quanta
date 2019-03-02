using System;

namespace Quanta.DataAccess.Entities
{
    public class DeviceAccess
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid DeviceId { get; set; }

        public Device Device { get; set; }

        public DateTime AccessedOn { get; set; }
    }
}
