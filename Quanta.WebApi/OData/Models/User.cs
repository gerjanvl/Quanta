﻿using System;
using System.Collections.Generic;
using Microsoft.AspNet.OData.Query;

namespace Quanta.WebApi.OData.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public Guid UserIdentity { get; set; }

        [NotExpandable]
        public List<UserDevice> Devices { get; set; }

        [NotExpandable]
        public List<UserDevice> RecentDevices { get; set; }
    }
}
