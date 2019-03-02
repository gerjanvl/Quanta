using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quanta.WebApi.Configuration
{
    public class Constants
    {
        public class Api
        {
            public const string V1 = "1.0";

            public const string ApplicationJson = "application/json";

            public class Routes
            {
                public const string Devices = "devices";

                public const string Me = "me";

                public const string Users = "users";
            }

            public class Roles
            {
                public const string Admin = nameof(Admin);

                public const string User = nameof(User);

                public const string Manager = nameof(Manager);

                public const string ManagerAndAdmin = "Admin, Manager";
            }
        }
    }
}
