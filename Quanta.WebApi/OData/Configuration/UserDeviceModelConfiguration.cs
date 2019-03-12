using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;
using Quanta.WebApi.Configuration;
using Quanta.WebApi.OData.Models;

namespace Quanta.WebApi.OData.Configuration
{
    public class UserDeviceModelConfiguration : IModelConfiguration
    {
        private static readonly ApiVersion V1 = new ApiVersion(1, 0);

        private EntityTypeConfiguration ConfigureCurrent(ODataModelBuilder builder)
        {
            var deviceConfiguration = builder.AddEntityType(typeof(UserDevice));

            return deviceConfiguration;
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
