﻿namespace Quanta.DataAccess.Models
{
    public class Device
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public string OperatingSystem { get; set; }

        public bool Enabled { get; set; }
    }
}