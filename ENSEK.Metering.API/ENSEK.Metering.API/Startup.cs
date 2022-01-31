using ENSEK.Metering.Repositories;
using ENSEK.Metering.Repositories.Interfaces;
using ENSEK.Metering.Services;
using ENSEK.Metering.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ENSEK.Metering.API
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
            var connnectionString = Configuration["ENSEKConnStr"];
            var sqlRepository = new SqlRepository(connnectionString);

            services.AddControllers();
            //services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<IDataService, FakeDataService>();
            services.AddSingleton<ISqlRepository>(sqlRepository);
            services.AddSingleton<IMeterReadingRepository, MeterReadingRepository>();
            services.AddSingleton<IValidationService, ValidationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
