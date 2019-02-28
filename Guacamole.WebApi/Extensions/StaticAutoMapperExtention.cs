using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Guacamole.WebApi.Extensions
{
    public static class StaticAutoMapperExtension
    {
        public static void AddAutoMapperStatic(this IServiceCollection services, Action<IMapperConfigurationExpression> configure)
        {
            Mapper.Initialize(configure);

            services.AddAutoMapper();
        }
    }
}
