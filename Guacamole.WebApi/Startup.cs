using System.Collections.Generic;
using System.Threading.Tasks;
using Guacamole.Client;
using Guacamole.DataAccess;
using Guacamole.Domain.Services;
using Guacamole.WebApi.Extensions;
using Guacamole.WebApi.Hubs;
using Guacamole.WebApi.MappingProfiles;
using Guacamole.WebApi.Swagger;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OData;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Guacamole.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("DefaultPolicy", builder =>
            {
                builder.WithOrigins("https://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.Authority = Configuration["AzureAd:Authority"];
                    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidAudiences = new List<string>
                        {
                            Configuration["AzureAd:ClientId"]
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

            services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddDbContext<GuacamoleContext>(o => o.UseSqlServer(Configuration.GetConnectionString("GuacamoleDatabase")));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<UserService>();
            services.AddScoped<DeviceService>();

            services.AddSingleton<GuacamoleClientManager>();
            services.AddScoped<GuacamoleClientConnectionManager>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddAutoMapperStatic(o => o.AddProfile<DomainProfile>());

            services.AddODataApiExplorer(options => { options.AssumeDefaultVersionWhenUnspecified = true; });
            services.AddApiVersioning(options => options.ReportApiVersions = true);
            services.AddOData().EnableApiVersioning();

            services.AddResponseCompression();

            services.AddSwaggerGen();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, VersionedODataModelBuilder modelBuilder, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("DefaultPolicy");

            app.UseAuthentication();

            app.UseSignalR(option => { option.MapHub<GuacamoleHub>("/ws"); });

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.SetUrlKeyDelimiter(ODataUrlKeyDelimiter.Slash);
                routeBuilder.Count().Filter().OrderBy().Expand().Select().MaxTop(100);
                routeBuilder.MapVersionedODataRoutes("odata-bypath", "api/v{version:apiVersion}", modelBuilder.GetEdmModels());
            });

            app.UseResponseCompression();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
        }
    }
}
