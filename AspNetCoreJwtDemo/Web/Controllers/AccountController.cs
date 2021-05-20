using Web.ViewModels;
using Web.ErrorModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Filters;
using Microsoft.Extensions.Logging;
using Web.Exceptions;
using Web.Services;
using Web.ConfigModels;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Security.Claims;

namespace Web.Controllers
{
    [Route("account")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        private readonly AppUserAuthConfigModel _appUserAuthConfig;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "To login using username and password",
            Description = "This API will authenticate a user and will return access token and a refresh token. Mobile app must store this tokens for subsequent API calls. Use the refresh token to get a new access token",
            OperationId = "UserLoginAsync",
            Tags = new[] { "Accounts" }
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(SimpleMessageResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationResultModel))]
        [ValidateModel]
        public async Task<ActionResult<LoginResponse>> UserLoginAsync([FromBody] LoginRequest loginRequest)
        {
            var loginResponse = await _accountService.LoginUserAsync(loginRequest);

            if (loginResponse == null)
            {
                _logger.LogWarning($"Login operation failed for username '{loginRequest.Username}'");
                throw new UnauthorizedException("Invalid user credentials");
            }

            return loginResponse;
        }

        [AllowAnonymous]
        [HttpGet("isalive")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<string> IsAlive()
        {
            return await Task<string>.FromResult("Is Alive");
        }

        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<string> GetProfile()
        {
            return await Task.FromResult("This is my profile");
        }

        [HttpGet("status")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<string> GetStatus()
        {

            var nameIdClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if(nameIdClaim != null)
            {
                return await Task.FromResult("NameId claim: "+nameIdClaim.Value);
            }

            return await Task.FromResult("This is for free users");
        }
    }
}
