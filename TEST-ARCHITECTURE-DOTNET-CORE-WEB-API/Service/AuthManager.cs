using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Data;
using TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Models;

namespace TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Service
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User _user;

        public AuthManager(UserManager<User> userManager, IConfiguration _configuration)
        {
            this._configuration = _configuration;
            this._userManager = userManager;
        }

        public async Task<bool> ValidateUser(UserLoginDto userDto)
        {
            _user = await _userManager.FindByEmailAsync(userDto.Email);
            return (_user != null && await _userManager.CheckPasswordAsync(_user, userDto.Password));
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials,claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials,List<Claim> authClaims)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var validIssuer = _configuration["JWT:ValidIssuer"];
            var validAudience = _configuration["JWT:ValidAudience"];
            var token = new JwtSecurityToken(
                issuer: validIssuer,
                audience: validAudience,
                claims: authClaims,
                signingCredentials:signingCredentials,
                expires: DateTime.Now.AddMinutes(15)
            );

            return token;
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim("user_id", _user.Id)
            };

            var roles = await this._userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,role));
            }

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = _configuration["JWT:SECRET_KEY"];
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
    }
}
