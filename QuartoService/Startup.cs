using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using QuartoService.Data;
using QuartoService.SyncDataServices.Http;

namespace QuartoService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsProduction())
            {
                Console.WriteLine("--> Using SqlServer Db");
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("QuartoConn")));
            }
            else
            {
                Console.WriteLine("--> Using InMem Db");
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseInMemoryDatabase("InMem"));
            }

            services.AddScoped<IQuartoRepo, QuartoRepo>();

            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Quartos", Version = "1.0.0" });

                var xmlFile = "QuartoService.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                //c.ExampleFilters();
            });

            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin(); // For anyone access.
            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });

            Console.WriteLine($"--> CommandService Endpoint {Configuration["CommandService"]}");
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuartoService v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("SiteCorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepDb.PrepPopulation(app, env.IsProduction());
            await HttpHotelDataClient.GetAllHotels(app);
        }
    }
}