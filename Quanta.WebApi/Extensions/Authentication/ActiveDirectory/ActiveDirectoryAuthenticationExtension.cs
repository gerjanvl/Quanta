using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace Quanta.WebApi.Extensions.Authentication.ActiveDirectory
{
    public static class ActiveDirectoryAuthenticationExtension
    {
        public static IServiceCollection AddActiveDirectoryAuthentication(this IServiceCollection services, Action<ActiveDirectoryAuthenticationOptions> configure)
        {
            var options = new ActiveDirectoryAuthenticationOptions();

            configure(options);

            return AddActiveDirectoryAuthentication(services, options);
        }

        public static IServiceCollection AddActiveDirectoryAuthentication(this IServiceCollection services, ActiveDirectoryAuthenticationOptions options)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.Authority = options.Authority;
                    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidAudiences = new List<string>
                        {
                            options.ClientId
                        }
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
