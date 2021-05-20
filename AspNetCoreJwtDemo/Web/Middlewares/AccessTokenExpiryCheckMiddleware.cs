using Web.ErrorModels;
using Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Middlewares
{
    public class AccessTokenExpiryCheckMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<AccessTokenExpiryCheckMiddleware> _logger;

        public AccessTokenExpiryCheckMiddleware(RequestDelegate next, ILogger<AccessTokenExpiryCheckMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var expClaim = context.User.Claims.FirstOrDefault(x => x.Type == "exp");

            if (expClaim != null)
            {
                DateTime expDate = EpochTime.DateTime(Convert.ToInt64(Math.Truncate(Convert.ToDouble(expClaim.Value, CultureInfo.InvariantCulture))));

                if (expDate < DateTime.UtcNow)
                {
                    var result = JsonHelper.ConvertToJson(new SimpleMessageResponseModel("AccessToken expired"));
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync(result);
                }
            }

            await _next.Invoke(context);
        }
    }
}