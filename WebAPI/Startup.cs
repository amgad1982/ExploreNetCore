using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebAPI.Configurations;
using WebAPI.Services;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebAPI {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMediatR(typeof(Startup).Assembly);
            services.RegisterAsemblyTypeHavingRegistrar<IService>((s) => { s.RegisterService(services); }, typeof(Startup).Assembly);
            //     services.RegisterAsemblyTypes(new List<Tuple<Type, Assembly, Action<dynamic>>>{
            //         Tuple.Create<Type, Assembly, Action<dynamic>>(typeof(IService),typeof(Startup).Assembly,(s)=>{s.RegisterService(services);})
            // });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(ops =>
                {
                    //ops.lo
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }
            #region Swagger

            var swaggerConfig = Configuration.GetConfig<SwaggerConfig> ();
            app.UseSwagger (options => {
                options.RouteTemplate = swaggerConfig.JsonRoute;
            });
            app.UseSwaggerUI (options => {
                options.SwaggerEndpoint (swaggerConfig.UiEndpoint, swaggerConfig.Description);
            });

            #endregion
            app.UseHttpsRedirection ();

            app.UseRouting ();

            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}