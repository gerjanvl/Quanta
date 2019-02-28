using System.Collections.Generic;

namespace Quanta.DataAccess.Models
{
    public class Device
    {
        public Device()
        {
            DeviceAccess = new HashSet<DeviceAccess>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public string OperatingSystem { get; set; }

        public bool Enabled { get; set; }

        public ICollection<DeviceAccess> DeviceAccess { get; set; }
    }
}
