using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Quanta.DataAccess;
using Quanta.Domain;
using Quanta.Extensions.Authentication.ActiveDirectory;
using Quanta.Infrastructure.Guacamole;
using Quanta.Infrastructure.Services;
using Quanta.SignalR.Hubs;

namespace Quanta.SignalR
{
    public class Startup
    {
        public const string DefaultPolicy = "DefaultPolicy";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<QuantaContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("QuantaDatabase")));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISessionService, SessionService>();

            services.AddSingleton<GuacamoleClientManager>();
            services.AddSingleton<GuacamoleClientConnectionManager<GuacamoleHub>>();

            services.AddAutoMapper();

            services.AddActiveDirectoryAuthentication(Configuration.GetSection("AzureAd")
                .Get<ActiveDirectoryAuthenticationOptions>());

            services.AddCors(c =>
                c.AddPolicy(DefaultPolicy, builder =>
                {
                    builder.WithOrigins("https://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                })
            );

            services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseCors(DefaultPolicy);

            app.UseAuthentication();

            app.UseSignalR(option => { option.MapHub<GuacamoleHub>("/ws"); });
        }
    }
}