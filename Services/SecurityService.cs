using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class SecurityService
    {
        private readonly IConfiguration configuration;

        public SecurityService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>(){
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.Username)
            };


            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
               issuer: configuration["Jwt:Issuer"],
               audience: configuration["Jwt:Issuer"],
               claims: claims,
               expires: DateTime.Now.AddMinutes(30),
               signingCredentials: signinCredentials
           );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        public int GetUserId(HttpContext httpContext)
        {
            Claim claim = GetJTIClaim(httpContext);
            return Int32.Parse(claim.Value);
        }

        private Claim GetJTIClaim(HttpContext httpContext)
        {
            IEnumerable<Claim> currentlyLoggedUserClaims = httpContext.User.Claims;

            Claim jtiClaim = currentlyLoggedUserClaims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
            return jtiClaim;
        }
    }
}
