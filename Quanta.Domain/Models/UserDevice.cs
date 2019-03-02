using System;

namespace Quanta.Domain.Models
{
    public class UserDevice
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid DeviceId { get; set; }

        public Device Device { get; set; }
    }
}
