using Fanda.Authentication.Repository;
using Fanda.Authentication.Repository.Dto;
using Fanda.Authentication.Repository.ViewModels;
using Fanda.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Fanda.Authentication.Service.Controllers
{
    // [Route("api/[controller]")]
    // [ApiController]
    public class AuthController : BaseController
    {
        private const string ModuleName = "User";
        private readonly IAuthRepository _repository;

        public AuthController(IAuthRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<AuthenticateResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] AuthenticateRequest model)
        {
            try
            {
                var response = await _repository.AuthenticateAsync(model, IpAddress());
                if (response == null)
                {
                    return BadRequest(MessageResponse.Failure("User name or password is incorrect"));
                }

                SetTokenCookie(response.RefreshToken);
                return Ok(DataResponse<AuthenticateResponse>.Succeeded(response));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
            }
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<AuthenticateResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return BadRequest(MessageResponse.Failure("Token is required"));
                }
                var response = await _repository.RefreshTokenAsync(refreshToken, IpAddress());
                if (response == null)
                {
                    return Unauthorized(MessageResponse.Failure("Invalid token"));
                }

                SetTokenCookie(response.RefreshToken);
                return Ok(DataResponse<AuthenticateResponse>.Succeeded(response));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
            }
        }

        [HttpPost("revoke-token")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(MessageResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            try
            {
                // accept token from request body or cookie
                var token = model.Token ?? Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(MessageResponse.Failure(errorMessage: "Token is required"));
                }
                var response = await _repository.RevokeTokenAsync(token, IpAddress());
                if (!response)
                {
                    return NotFound(MessageResponse.Failure(errorMessage: "Invalid token"));
                }
                return Ok(MessageResponse.Succeeded(message: "Token revoked"));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<UserDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                var validationResult = await _repository.ValidateAsync(model.TenantId, model);
                if (validationResult.IsValid())
                {
                    string token = Guid.NewGuid().ToString();
                    string callbackUrl = Url.Page(
                        pageName: "/Users/ConfirmEmail",
                        pageHandler: null,
                        values: new { userName = model.UserName, code = token },
                        protocol: Request.Scheme
                    );

                    // save
                    var userDto = await _repository.RegisterAsync(model, callbackUrl);
                    return Ok(DataResponse<UserDto>.Succeeded(userDto));
                }
                else
                {
                    return BadRequest(MessageResponse.Failure(validationResult));
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
            }
        }

        //[HttpGet("{userId}/refresh-tokens")]
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(DataResponse<List<ActiveTokenViewModel>>), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> GetRefreshTokens([Required] Guid userId)
        //{
        //    try
        //    {
        //        var tokens = await _repository.GetRefreshTokens(userId);
        //        return Ok(DataResponse<IEnumerable<ActiveTokenViewModel>>.Succeeded(tokens));
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex, ModuleName);
        //    }
        //}

        #region helper methods

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }

        #endregion helper methods
    }
}
