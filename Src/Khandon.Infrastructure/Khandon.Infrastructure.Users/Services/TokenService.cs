﻿using Khandon.Core.Interfaces.User.Identity;
using Khandon.Infrastructure.Users.DataContext;
using Khandon.Infrastructure.Users.Models.User;
using Khandon.Shared.Dto.Request.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.Infrastructure.Users.Services
{
    public class TokenService: ITokenService
    {

        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly UserDbContext userDb;

        private readonly JwtConfig _jwtConfig;

        public TokenService(IOptionsMonitor<JwtConfig> optionsMonitor, UserDbContext userDb, RoleManager<ApplicationRole> roleManager)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
            this.userDb = userDb;
            _roleManager = roleManager;
        }

        public TokenDto GenrateToken(ApplicationUser user)
        {
            // Now its ime to define the jwt token which will be responsible of creating our tokens
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // We get our secret from the appsettings
            var Secretkey = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Secretkey), SecurityAlgorithms.HmacSha512Signature);

            var EncryptionKey = Encoding.ASCII.GetBytes(_jwtConfig.EncryptionKey);
            var EncryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(EncryptionKey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            // we define our token descriptor
            // We need to utilise claims which are properties in our token which gives information about the token
            // which belong to the specific user who it belongs to
            // so it could contain their id, name, email the good part is that these information
            // are generated by our server and identity framework which is valid and trusted
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                    // the JTI is used for our refresh token which we will be convering in the next video
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                // the life span of the token needs to be shorter and utilise refresh token to keep the user signedin
                // but since this is a demo app we can extend it to fit our current need
                Expires = DateTime.UtcNow.AddDays(50),
                // here we are adding the encryption alogorithim information which will be used to decrypt our token
                SigningCredentials = SigningCredentials,
                //add jwt excriptin
                EncryptingCredentials = EncryptingCredentials
            };
            var roles = _roleManager.Roles.ToList();
            foreach (var item in userDb.UserRoles.Where(a => a.UserId == user.Id))
            {
                tokenDescriptor.Subject.AddClaim(new(ClaimTypes.Role, roles.FirstOrDefault(a => item.RoleId == a.Id).Name));
            }

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return new TokenDto()
            {
                ExpireDateUtc = tokenDescriptor.Expires.Value,
                Token = jwtToken
            };
        }

    }
}