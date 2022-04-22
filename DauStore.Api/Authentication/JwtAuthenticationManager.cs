using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IServices;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DauStore.Api.Authentication
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly string key = "This is my test key";
        protected IBaseService<User> _userService;
        protected IBaseService<TokenAccount> _tokenAccountService;

        public JwtAuthenticationManager(IBaseService<User> userService, IBaseService<TokenAccount> tokenAccountService)
        {
            _userService = userService;
            _tokenAccountService = tokenAccountService;
        }

        public string Authenticate(string phoneNumber, string password)
        {
            if (!CheckAccountLogin(phoneNumber, password))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, phoneNumber)
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);


            var ra = _tokenAccountService.DeleteByProp("Phone", phoneNumber);
            var result = _tokenAccountService.Add(new TokenAccount(phoneNumber, $"bearer {tokenHandler.WriteToken(token)}"));


            if (result.StatusCode == 201)
            {
                return $"bearer {tokenHandler.WriteToken(token)}";
            }
            else
            {
                return null;
            }
        }

        public Boolean CheckAccountLogin(string phoneNumber, string password)
        {
            var result = _userService.GetByProp("PhoneNumber", phoneNumber);
            if (result.Response.Data == null)
            {
                return false;
            }
            else
            {
                User acc = (User)result.Response.Data;
                bool verified = BCrypt.Net.BCrypt.Verify(password, acc.Password);
                if (verified)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
