using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class LoginResponse
    {
        public int AppUserId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiryUtc { get; set; }
        public DateTime RefreshTokenExpiryUtc { get; set; }
    }
}
