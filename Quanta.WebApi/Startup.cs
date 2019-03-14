using System.Collections.Generic;
using System.Diagnostics;
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
using Quanta.Domain;
using Quanta.Extensions.Authentication.ActiveDirectory;
using Quanta.Extensions.AutoMapper;
using Quanta.Infrastructure.Guacamole;
using Quanta.Infrastructure.Services;
using Quanta.WebApi.Configuration.MappingProfiles;
using Quanta.WebApi.Configuration.Swagger;
using Quanta.WebApi.OData.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Quanta.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<QuantaContext>(c => c.UseSqlServer(Configuration.GetConnectionString("QuantaDatabase")));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<ISessionService, SessionService>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddAutoMapperStatic(o => o.AddProfile<DomainProfile>());

            services.AddCors();

            services.AddActiveDirectoryAuthentication(Configuration.GetSection("AzureAd").Get<ActiveDirectoryAuthenticationOptions>());

            services.AddODataApiExplorer(options => options.AssumeDefaultVersionWhenUnspecified = true);
            services.AddApiVersioning(options => options.ReportApiVersions = true);
            services.AddOData().EnableApiVersioning();

            services.AddSwaggerGen();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddResponseCompression();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, VersionedODataModelBuilder modelBuilder, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());

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
