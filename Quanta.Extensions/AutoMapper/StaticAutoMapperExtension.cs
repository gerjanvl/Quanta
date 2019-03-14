using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Quanta.Extensions.AutoMapper
{
    public static class StaticAutoMapperExtension
    {
        public static IServiceCollection AddAutoMapperStatic(this IServiceCollection services, Action<IMapperConfigurationExpression> configure)
        {
            Mapper.Initialize(configure);

            services.AddAutoMapper();

            return services;
        }
    }
}
