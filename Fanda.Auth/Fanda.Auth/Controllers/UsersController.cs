using Fanda.Core;
using Fanda.Core.Base;
using Fanda.Core.Extensions;
using FandaAuth.Service;
using FandaAuth.Service.Dto;
using FandaAuth.Service.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Fanda.Authentication.Controllers
{
    //[EnableCors("_MyAllowedOrigins")]
    //[Authorize]
    //[Produces(MediaTypeNames.Application.Json)]
    //[ApiController]
    //[Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private const string ModuleName = "User";
        private readonly IUserRepository _repository;

        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<AuthenticateResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            try
            {
                var response = await _repository.Authenticate(model, IpAddress());
                if (response == null)
                {
                    return BadRequest(MessageResponse.Failure("Username or password is incorrect"));
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
                var response = await _repository.RefreshToken(refreshToken, IpAddress());
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
                var response = await _repository.RevokeToken(token, IpAddress());
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
        [ProducesResponseType(typeof(DataResponse<UserDto>), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                var validationResult = await _repository.ValidateAsync(model.TenantId, model);
                if (validationResult.IsValid)
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
                    return CreatedAtAction(nameof(GetById), new { id = userDto.Id },
                        DataResponse<UserDto>.Succeeded(userDto));
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

        [HttpGet("{id}/refresh-tokens")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<List<ActiveTokenDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRefreshTokens([Required] Guid id)
        {
            try
            {
                var tokens = await _repository.GetRefreshTokens(id);
                return Ok(DataResponse<IEnumerable<ActiveTokenDto>>.Succeeded(tokens));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
            }
        }

        // users/all/5
        [HttpGet("all/{tenantId}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<List<UserListDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([Required] Guid tenantId)
        {
            try
            {
                NameValueCollection queryString = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var query = new Query(queryString["page"], queryString["pageSize"])
                {
                    Filter = queryString["filter"],
                    FilterArgs = queryString["filterArgs"]?.Split(','),
                    Search = queryString["search"],
                    Sort = queryString["sort"],
                };

                var response = await _repository.GetData(tenantId, query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
            }
        }

        // users/5
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<UserDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([Required, FromRoute] Guid id, [FromQuery] bool include)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id, include);
                if (user == null)
                {
                    return NotFound(MessageResponse.Failure($"{ModuleName} id '{id}' not found"));
                }
                return Ok(DataResponse<UserDto>.Succeeded(user));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
            }
        }

        [HttpPost("{tenantId}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(DataResponse<UserDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create(Guid tenantId, UserDto model)
        {
            try
            {
                #region Validation

                var validationResult = await _repository.ValidateAsync(tenantId, model);

                #endregion Validation

                if (validationResult.IsValid)
                {
                    var userDto = await _repository.CreateAsync(tenantId, model);
                    return CreatedAtAction(nameof(GetById), new { id = model.Id },
                        DataResponse<UserDto>.Succeeded(userDto));
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

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update(Guid id, UserDto model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest(MessageResponse.Failure($"{ModuleName} Id mismatch"));
                }

                #region Validation

                var validationResult = await _repository.ValidateAsync(id, model);

                #endregion Validation

                if (validationResult.IsValid)
                {
                    await _repository.UpdateAsync(id, model);
                    return NoContent();
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

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty)
                {
                    return BadRequest(MessageResponse.Failure($"{ModuleName} id is missing"));
                }
                var success = await _repository.DeleteAsync(id);
                if (success)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound(MessageResponse.Failure($"{ModuleName} not found"));
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex, ModuleName);
            }
        }

        #region helper methods

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions
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
