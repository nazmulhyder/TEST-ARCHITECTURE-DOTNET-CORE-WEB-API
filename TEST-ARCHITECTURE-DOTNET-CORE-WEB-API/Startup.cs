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
using Microsoft.EntityFrameworkCore;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Configuration;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Data;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.IRepository;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Repository;

namespace TEST_ARCHITECTURE_DOTNET_CORE_WEB_API
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
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DBConnectionString"))
            );
            services.AddCors(cors =>
            {
                cors.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddAutoMapper(typeof(MapperInitializer));

            services.AddTransient<IUnitOfWork,UnitOfWork>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DOT NET CORE WEB API ARCHITECTURE", Version = "v1" });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

          
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DOT NET CORE WEB API ARCHITECTURE"));

            app.UseCors("CorsPolicy");

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
