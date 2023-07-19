using IdentityService.Api.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityService.Api.Services
{
    public class IdentityService : IIdentityService
    {
        public Task<LoginResponseModel> Login(LoginRequestModel requestModel)
        {
            //DB Process will be here. check if user information is valid and get details


            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, requestModel.UserName),
                new Claim(ClaimTypes.Name, "ismail M."),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MyAwesomeSecretKeySupppperrrrrHarddddddddddd"));
            var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddMinutes(90);

            var token = new JwtSecurityToken(claims: claims, expires: expiry, signingCredentials: creds, notBefore: DateTime.Now);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            LoginResponseModel response = new()
            {
                UserName = requestModel.UserName,
                Token = encodedJwt,
            };

            return Task.FromResult(response);
        }


    }
}
 
