using System;

namespace Quanta.Domain.UserDevice
{
    public class UserDevice
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User.User User { get; set; }

        public Guid DeviceId { get; set; }

        public Device.Device Device { get; set; }
    }
}
