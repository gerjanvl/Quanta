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
            var userConfiguration = builder.AddEntityType(typeof(User));

            builder.AddEntitySet("Users", userConfiguration);
            builder.AddSingleton("Me", userConfiguration);

            return userConfiguration;
        }

        public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {
            if (apiVersion == V1)
            {
                ConfigureCurrent(builder);
            }
        }
    }
}
