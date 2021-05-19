using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Web.ConfigModels;
using Web.ViewModels;

namespace Web.Services
{
    public class AccountService: IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly AppUserAuthConfigModel _appUserAuthConfig;

        public AccountService(ILogger<AccountService> logger, IOptions<AppUserAuthConfigModel> appUserAuthOptions)
        {
            _appUserAuthConfig = appUserAuthOptions.Value;
            _logger = logger;
        }

        public async Task<LoginResponse> LoginUserAsync(LoginRequest loginRequest)
        {
            var validUsername = "admin";
            var validPassword = "admin";

            if (loginRequest.Username == validUsername && loginRequest.Password == validPassword)
            {
                var loginResponse = new LoginResponse();
                loginResponse.AppUserId = 100;
                var accessTokenExpiryUtc = DateTime.UtcNow.AddMinutes(_appUserAuthConfig.AccessTokenExpiryInMinutes);
                loginResponse.AccessTokenExpiryUtc = accessTokenExpiryUtc;
                loginResponse.AccessToken = GenerateAccessToken( accessTokenExpiryUtc);

                loginResponse.RefreshTokenExpiryUtc = DateTime.UtcNow.AddHours(_appUserAuthConfig.RefreshTokenExpiryInHours);
                loginResponse.RefreshToken = GenerateRefreshToken();

                return await Task<LoginResponse>.FromResult(loginResponse);
            }

            return null;
        }

        private string GenerateAccessToken(DateTime expiryDate)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appUserAuthConfig.JwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "100"),
                    new Claim(ClaimTypes.Name,"Pradeep"),
                    new Claim(ClaimTypes.Email, "123@123.com")
                }),
                Audience = _appUserAuthConfig.Audience,
                Issuer = _appUserAuthConfig.Issuer,
                Expires = expiryDate, // DateTime.UtcNow.AddMinutes(_appUserAuthConfig.AccessTokenExpiryInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

    }
}
