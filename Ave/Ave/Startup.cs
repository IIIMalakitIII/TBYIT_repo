using AutoMapper;
using Ave.API.Configuration;
using Ave.API.Mappings;
using Ave.API.Middleware;
using Ave.Common.Authentication;
using Ave.DAL.Context;
using Ave.DAL.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ave.API
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
            services.AddAutoMapper(typeof(MappingProfile));

            services.ConfigureDbContext(Configuration);

            services.ConfigureIdentity();

            services.Configure<JwtSettings>(Configuration.GetSection("Auth"));

            services.AddJwtAuthentication(Configuration);

            services.ConfigureAuthorization();

            services.ConfigureDiServices();

            services.AddScoped<SaveMiddleware>();

            services.AddCors();

            services.AddControllers();

            services.ConfigureSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseMiddleware<SaveMiddleware>();

            app.UseCors(options =>
                options.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "EReceiptApi"));

            app.MigrateDataBase();
        }
    }
}
