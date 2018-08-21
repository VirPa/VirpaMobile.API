using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Virpa.Mobile.BLL.v1.ConfigServices;
using Virpa.Mobile.BLL.v1.Middlewares;
using Virpa.Mobile.DAL.v1.Model;

namespace Virpa.Mobile.API.v1 {

    public class Startup {

        private static string _dbConnection;
        private readonly IConfigurationRoot _configurationRoot;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("Manifest\\manifest.json", false, true);

            _configurationRoot = builder.Build();

            _dbConnection = _configurationRoot.GetConnectionString("DefaultConnection");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.RegisterIdentities(_dbConnection);

            services.RegisterAuthentication();

            services.RegisterMvc();

            services.AddApiVersioning(api => {
                api.AssumeDefaultVersionWhenUnspecified = true;
                api.DefaultApiVersion = new ApiVersion(1, 0);
                api.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            services.RegisterMapper();

            services.RegisterDInjection();

            services.RegisterSwagger();

            services.AddOptions();

            services.Configure<Manifest>(_configurationRoot.GetSection("Info"));
            services.Configure<Manifest>(_configurationRoot.GetSection("Manifest"));
            services.Configure<Manifest>(_configurationRoot.GetSection("ConnectionStrings"));
            services.Configure<Manifest>(_configurationRoot.GetSection("JwtTokens"));
            services.Configure<Manifest>(_configurationRoot.GetSection("EmailNotification"));
            services.Configure<Manifest>(_configurationRoot.GetSection("Files"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {

            loggerFactory.AddConsole();

            //if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();

                app.UseStatusCodePages();
            //}

            app.RegisterStaticFiles();

            app.RegisterSwagger();

            app.UseAuthentication();

            app.UseSwaggerAuthentication();

            app.UseMvc();

            app.Run(async context => {
                await context.Response.WriteAsync(
                    "Welcome To VirPA Technology API. Last updated July 02, 2018 03:55PM GMT+8");
            });
        }
    }
}
