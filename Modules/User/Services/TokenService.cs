using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using myapp.Modules.User.Interface;
using myapp.Modules.User.Models;

namespace myapp.Modules.User.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly  SymmetricSecurityKey _key;
        private readonly IEncryptionService _encryptionService;
        public TokenService(IConfiguration config, IEncryptionService encryptionService)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            _encryptionService = encryptionService;

        }
        public string CreateToken(AppUser appUser)
        {

            string encryptedEmail = _encryptionService.EncryptData(appUser.Email);
            string encryptedUserName = _encryptionService.EncryptData(appUser.UserName);
            string encryptedUserId = _encryptionService.EncryptData(appUser.Id);


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, encryptedEmail),
                new Claim(JwtRegisteredClaimNames.GivenName, encryptedUserName),
                new Claim(JwtRegisteredClaimNames.Sub, encryptedUserId) 
            };


            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
