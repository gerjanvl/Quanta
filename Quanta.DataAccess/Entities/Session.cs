using System;
using System.Collections.Generic;
using System.Text;

namespace Quanta.DataAccess.Entities
{
    public class Session
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid DeviceId { get; set; }

        public Device Device { get; set; }

        public DateTime? FinishedOn { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
