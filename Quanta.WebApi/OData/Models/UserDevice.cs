using System;

namespace Quanta.WebApi.OData.Models
{
    public class UserDevice
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string OperatingSystem { get; set; }

        public bool Enabled { get; set; }
    }
}