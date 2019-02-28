﻿using System;
using System.Collections.Generic;

namespace Quanta.DataAccess.Models
{
    public class User
    {
        public int Id { get; set; }

        public Guid UserIdentity { get; set; }

        public List<UserDevice> UserDevices { get; set; }
    }
}