using System;
using System.Collections.Generic;

namespace Guacamole.WebApi.OData.Models
{
    public class User
    {
        public int Id { get; set; }

        public Guid UserIdentity { get; set; }

        public List<Device> Devices { get; set; }
    }
}
