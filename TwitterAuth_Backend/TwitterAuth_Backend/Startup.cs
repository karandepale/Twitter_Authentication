using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterAuth_Backend.Data;
using TwitterAuth_Backend.Model;


namespace TwitterAuth_Backend
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
            services.AddControllers();

            //SWAGGER:-
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Twitter SignIn",
                    Description = "Sign In API's",
                    Contact = new OpenApiContact
                    {
                        Name = "Karan Depale",
                        Email = "karandepale111@gmail.com",
                    },
                });
            });


            //CORS SERVICE CONFIGURATION:-
            services.AddCors(data =>
            {
                data.AddPolicy(
                    name: "AllowOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });

            //TWITTER CONFIGURATION:-
            services.Configure<TwitterSettings>(Configuration.GetSection("TwitterSettings"));
            services.AddHttpClient("twitter");
            services.AddScoped<ITwitterAuthRepository, TwitterAuthRepository>();



        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            //SWAGGER MIDDLEWARE:-
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Twitter SignIn V1");
            });




            //CORS MIDDLEWARE:-
            app.UseCors("AllowOrigin");




            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
