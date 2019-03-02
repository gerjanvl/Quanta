using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;
using Quanta.WebApi.OData.Models;
using Quanta.WebApi.Configuration;

namespace Quanta.WebApi.OData.Configuration
{
    public class UserModelConfiguration : IModelConfiguration
    {
        private static readonly ApiVersion V1 = new ApiVersion(1, 0);

        private EntityTypeConfiguration ConfigureCurrent(ODataModelBuilder builder)
        {
            var userConfiguration = builder.AddEntityType(typeof(User));

            builder.AddEntitySet(Constants.Api.Routes.Users, userConfiguration);
            builder.AddSingleton(Constants.Api.Routes.Me, userConfiguration);

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
