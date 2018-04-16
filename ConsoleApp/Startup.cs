using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp
{

    //Typical net core startup class with the bonus of DI and Confiugration loader.. cool.
    public class Startup
    {

        //  MetaProgramming from http://stackoverflow.com/a/17227764/19020 to load controllers in 
        //  another assembly.  Another way to do this is to create a custom assembly resolver
        //   Type valuesControllerType = typeof(OWINTest.API.ValuesController);
        //   can be usefull for plugins?


        public Startup(IHostingEnvironment env, IConfiguration config)
        {
            HostingEnvironment = env;
            Configuration = config;
        }

        public IHostingEnvironment HostingEnvironment { get; }
        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            if (HostingEnvironment.IsDevelopment())
            {
                // Development configuration
            }
            else
            {
                // Staging/Production configuration
            }

            services.AddMvc();
        }

    }
}
