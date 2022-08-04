using Khandon.Core.Interfaces.User.Identity;
using Khandon.Infrastructure.Users.DataContext;
using Khandon.Infrastructure.Users.Helper;
using Khandon.Infrastructure.Users.Models.User;
using Khandon.Infrastructure.Users.Services;
using Khandon.Shared.Dto.Base.User;
using Khandon.Shared.Dto.Request.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Infrastructure.Users
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUsersServices(this IServiceCollection services,IConfiguration Configuration)
        {

            //add jwt
            //must 16 char
            var Secretkey = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);
            //must be long
            var EncryptionKey = Encoding.ASCII.GetBytes(Configuration["JwtConfig:EncryptionKey"]);

            //paramer to decode token
            var TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true, // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings and verify we have generated the jwt token
                IssuerSigningKey = new SymmetricSecurityKey(Secretkey), // Add the secret key to our Jwt encryption
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,//make sense token was expir in time
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                TokenDecryptionKey = new SymmetricSecurityKey(EncryptionKey)//this for decode token readblity.
            };
            //inject authentication setting
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = TokenValidationParameters;

            });
            //inject JwtConfig object for using this value in controller
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));


            //Register identity service
            services.AddIdentity<ApplicationUser,ApplicationRole>(options =>
            {
                //Change identity defult setting. For password and more
                options.SignIn.RequireConfirmedEmail = true;

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
                
                //options.Tokens.EmailConfirmationTokenProvider = "theemail";
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
             .AddEntityFrameworkStores<UserDbContext>()
             .AddDefaultTokenProviders()
             .AddErrorDescriber<PersianIdentityErrorDescriber>();

            services.AddDbContext<UserDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString: Configuration.GetConnectionString("UserDb"));
            });

            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
            services.Configure<EmailConfig>(Configuration.GetSection("EmailConfig"));

            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IEmailService, EmailService>();


            return services;
        }
    }
}
