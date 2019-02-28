namespace Guacamole.Data.Models
{
    public class UserDevice
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int DeviceId { get; set; }

        public Device Device { get; set; }
    }
}
