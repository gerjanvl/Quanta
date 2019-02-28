using System.Collections.Generic;
using System.Threading.Tasks;
using Guacamole.Client;
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
using Quanta.DataAccess;
using Quanta.Domain.Services;
using Quanta.WebApi.Extensions;
using Quanta.WebApi.Extensions.Authentication.ActiveDirectory;
using Quanta.WebApi.Hubs;
using Quanta.WebApi.MappingProfiles;
using Quanta.WebApi.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Quanta.WebApi
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

            services.AddActiveDirectoryAuthentication(Configuration.GetValue<ActiveDirectoryAuthenticationOptions>("AzureAd"));

            services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddDbContext<GuacamoleContext>(o => o.UseSqlServer(Configuration.GetConnectionString("QuantaDatabase")));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDeviceService, DeviceService>();

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
