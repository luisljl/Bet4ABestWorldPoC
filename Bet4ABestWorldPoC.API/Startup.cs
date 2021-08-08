using Bet4ABestWorldPoC.Repositories;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Repositories.Repositories;
using Bet4ABestWorldPoC.Services;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Shared.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Bet4ABestWorldPoC.API
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
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AppDbContext")
            ));

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IBetRepository, BetRepository>();
            services.AddTransient<IDepositRepository, DepositRepository>();
            services.AddTransient<IBlackListTokenRepository, BlackListTokenRepository>();
            services.AddTransient<ISlotRepository, SlotRepository>();
            services.AddTransient<IBalanceRepository, BalanceRepository>();
            // Services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBetService, BetService>();
            services.AddTransient<IDepositService, DepositService>();
            services.AddTransient<ISlotService, SlotService>();
            services.AddTransient<IBalanceService, BalanceService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IRegisterService, RegisterService>();
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bet4ABestWorldPoC.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext context)
        {

            context.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bet4ABestWorldPoC.API v1"));
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
