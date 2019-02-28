﻿using Guacamole.WebSocket.OData.Models;
using Guacamole.WebSocket.ViewModels;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Guacamole.WebSocket.OData.Configuration
{
    public class UserModelConfiguration : IModelConfiguration
    {
        private static readonly ApiVersion V1 = new ApiVersion(1, 0);

        private EntityTypeConfiguration ConfigureCurrent(ODataModelBuilder builder)
        {
            var userConfig = builder.AddEntityType(typeof(User));

            builder.AddEntitySet("Users", userConfig);
            builder.AddSingleton("Me", userConfig);

            return userConfig;
        }

        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {
            // note: the EDM for orders is only available in version 1.0
            if (apiVersion == V1)
            {
                ConfigureCurrent(builder);
            }
        }
    }
}
