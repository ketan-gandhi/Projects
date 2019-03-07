using CylanceGUID.Exceptions;
using CylanceGUID.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CylanceGUID
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GuidsDBContext>(s => s.UseSqlServer(Configuration["AppSettings:ConnectionString"])); 
            services.AddMvc(config=>
            {
                config.Filters.Add(typeof(CustomExceptionHandler));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<ICaching, Caching>();
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = "localhost:6379";
                //option.Configuration= Configuration.GetConnectionString("Redis");
                option.InstanceName = "Cyclance"; 
             
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
