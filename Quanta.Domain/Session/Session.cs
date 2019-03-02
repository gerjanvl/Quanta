using System;

namespace Quanta.Domain.Session
{
    public class Session
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User.User User { get; set; }

        public Guid DeviceId { get; set; }

        public Device.Device Device { get; set; }

        public DateTime? FinishedOn { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
