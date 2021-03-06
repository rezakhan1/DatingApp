using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entity;
using API.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Service
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration iconfig) {
            _key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(iconfig["TokenKey"]));
        }
        public string CreateToken(AppUser user)
        {
           var claim=new List<Claim>{
               new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName)
           };

           var cred=new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);

           var tokenDescriptor=new SecurityTokenDescriptor{
               Subject=new ClaimsIdentity(claim),
               Expires=DateTime.Now.AddDays(7),
               SigningCredentials=cred
           };
           var tokenHandler=new JwtSecurityTokenHandler();
           var token=tokenHandler.CreateToken(tokenDescriptor);
           return tokenHandler.WriteToken(token);

        }
    }
}