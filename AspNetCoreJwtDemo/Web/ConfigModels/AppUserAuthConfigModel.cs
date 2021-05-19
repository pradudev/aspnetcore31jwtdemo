using System;
using System.Collections.Generic;
using System.Text;

namespace Web.ConfigModels
{
    public class AppUserAuthConfigModel
    {
        public int AccessTokenExpiryInMinutes { get; set; }
        public int RefreshTokenExpiryInHours { get; set; }
        public string JwtSecretKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}
