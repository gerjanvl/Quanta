using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper.Extensions.ExpressionMapping.Extensions;

namespace Guacamole.WebSocket.Extensions
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
