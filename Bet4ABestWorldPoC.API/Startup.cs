using Bet4ABestWorldPoC.API.Middleware;
using Bet4ABestWorldPoC.Repositories;
using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Repositories.Repositories;
using Bet4ABestWorldPoC.Services;
using Bet4ABestWorldPoC.Services.Enums;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Shared.Settings;
using Bet4ABestWorldPoC.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

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
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBetService, BetService>();
            services.AddScoped<IDepositService, DepositService>();
            services.AddScoped<ISlotService, SlotService>();
            services.AddScoped<IBalanceService, BalanceService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRegisterService, RegisterService>();

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.JWTSecret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("Custom", x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Custom")
                    .Build();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bet4ABestWorldPoC.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Please insert JWT token into field"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext context)
        {
            var migrateDbPolicy = Policy
                   .Handle<Exception>()
                   .WaitAndRetry(4, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

            migrateDbPolicy.Execute(() =>
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                    GenerateFakeData(context);
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bet4ABestWorldPoC.API v1"));
            }

            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());


            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
            app.UseMiddleware(typeof(JwtMiddleware));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void GenerateFakeData(AppDbContext context)
        {
            var users = new List<User>();
            for (int i = 0; i < 100; i++)
            {
                users.Add(new User()
                {
                    CreatedOn = DateTime.Now.AddYears(-1).AddDays(1),
                    Email = $"email_user_{i}@gmail.com",
                    Password = Security.HashPassword($"password{i}"),
                    Username = $"user{i}"
                });
            }
            context.Users.AddRange(users);
            context.SaveChanges();

            var slots = new List<Slot>();
            for (int i = 0; i < 50; i++)
            {
                slots.Add(new Slot()
                {
                    Name = $"Slot {i} name",
                    RTP = 95,
                });
            }
            context.Slots.AddRange(slots);
            context.SaveChanges();

            var balances = new List<Balance>();
            users.ForEach((user) =>
            {
                balances.Add(new Balance()
                {
                    UserId = user.Id,
                    Amount = 0,
                });
            });

            var deposits = new List<Deposit>();
            users.ForEach((user) =>
            {
                double totalAmount = 0;
                for (int i = 0; i < new Random().Next(1, 5); i++)
                {
                    var amount = new Random().Next(10, 101);
                    deposits.Add(new Deposit()
                    {
                        MerchantId = (int)Merchant.PAYPAL,
                        UserId = user.Id,
                        Amount = amount,
                    });
                    totalAmount += amount;
                }
                balances.First(f => f.UserId == user.Id).Amount = totalAmount;
            });
            context.Deposits.AddRange(deposits);
            context.SaveChanges();
            var bets = new List<Bet>();
            users.ForEach((user) =>
            {
                for (int i = 0; i < new Random().Next(10, 1000); i++)
                {
                    var winningBet = new Random().Next(0, 5) > 3;
                    var winningAmount = winningBet ? new Random().NextDouble() * 0.5 : 0;
                    var bet = new Bet()
                    {
                        Amount = 0.10,
                        BetType = (int)BetType.NORMAL,
                        CreatedOn = DateTime.Now,
                        SlotId = new Random().Next(0, 50),
                        UserId = user.Id,
                        WinningBet = winningBet,
                        WinningAmount = winningAmount,
                    };
                    var balance = balances.First(f => f.UserId == user.Id).Amount;
                    balance += winningAmount != 0 ? (winningAmount - 0.10) : -0.10;
                    if (balance < 0)
                    {
                        balance = 0;
                    }
                    bets.Add(bet);
                }
            });

            context.Bets.AddRange(bets);
            context.Balances.AddRange(balances);
            context.SaveChanges();
        }
    }
}
