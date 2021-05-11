namespace StickersOnMap.WEB
{
    using System;
    using System.IO;
    using System.Reflection;
    using AutoMapper;
    using Core.Infrastructure;
    using DAL.DbContexts;
    using DAL.Interfaces;
    using DAL.Repositories;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using HealthChecks.UI.Client;
    using Infrastructure.MappingProfiles;
    using Infrastructure.Settings;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using NLog.Extensions.Logging;

    public class Startup
    {
        private const string StickerMemoryContext = "StickerMemoryDb";
        
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson(opts => opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSingleton<IAppSettings, AppSettings>();
            HealthChecks(services);
            ConfigureDb(services);
            ConfigLogging(services);
            ConfigureMapper(services);
            ConfigSwagger(services);
        }
        
        private void HealthChecks(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<StickersDb>(
                    name: StickerMemoryContext,
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] {"inMemory", "StickerMemoryDb"}
                );
        }
        
        private void ConfigureMapper(IServiceCollection services)
        {
            try
            {
                var mapperConfig = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfileSticker>();
                });
                
                var mapper = mapperConfig.CreateMapper();
                mapper.ConfigurationProvider.CompileMappings();
                services.AddSingleton(mapper);
            }
            catch (AutoMapperConfigurationException ex)
            {
                var message = "Ошибка в настройках конфигурации автомаппера. Не удалось собрать карту отображений моделей друг в друга.";
                throw new InvalidCastException(message, ex);
            }
        }
        
        private void ConfigureDb(IServiceCollection services)
        {
            services.AddDbContext<StickersDb>(options =>
            {
                options.UseInMemoryDatabase(StickerMemoryContext);
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<IStickerRepoFacade, StickerRepoFacade>();
        }
        
        private void ConfigLogging(IServiceCollection services)
        {
            services.AddLogging(builder => builder.AddNLog("nlog.config"));
        }
        
        private void ConfigSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "StickersOnMap.Web API",
                    Description = "ASP.NET Core Web API"
                });
                options.IncludeXmlComments(GetXmlCommentsPath());
            });
        }
        
        private static string GetXmlCommentsPath()
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            return Path.Combine(AppContext.BaseDirectory, xmlFile);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHsts();
            app.UseMiddleware<MiddlewareException>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks(Configuration["HealthCheckSettings:Path"],
                        new HealthCheckOptions
                        {
                            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                        });
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapSwagger();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
            
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "StickersOnMap API V1");
                options.RoutePrefix = string.Empty;
            });
        }
    }
}