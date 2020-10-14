using Fanda.Authentication.Domain;
using Fanda.Authentication.Repository;
using Fanda.Authentication.Repository.Dto;
using Fanda.Core.Base;

namespace Fanda.Authentication.Service.Controllers
{
    public class RolesController : //FandaControllerBase<IRoleRepository, RoleDto, RoleListDto, TenantKeyData>
        SubController<IRoleRepository, Role, RoleDto, RoleListDto>
    {
        //private const string ModuleName = "Role";
        //private readonly IRoleRepository repository;

        public RolesController(IRoleRepository repository) : base(repository)
        {
            //this.repository = repository;
        }

        // roles/5
        //[HttpGet("{id}")]
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        //[ProducesResponseType(typeof(DataResponse<RoleDto>), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> GetById([Required, FromRoute] Guid id/*, [FromQuery] bool include*/)
        //{
        //    try
        //    {
        //        var role = await repository.GetByIdAsync(id/*, include*/);
        //        if (role == null)
        //        {
        //            return NotFound(MessageResponse.Failure($"{ModuleName} id '{id}' not found"));
        //        }
        //        return Ok(DataResponse<RoleDto>.Succeeded(role));
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex, ModuleName);
        //    }
        //}

        // [HttpPost("add-privilege")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // public async Task<IActionResult> AddPrivilege([FromBody] PrivilegeDto model)
        // {
        //     try
        //     {
        //         bool success = await _repository.AddPrivilege(model);
        //         if (success)
        //         {
        //             return Ok(DataResponse.Succeeded("Mapped privilege successfully"));
        //         }
        //         return NotFound(DataResponse.Failure($"Privilege not found"));
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
        //         {
        //             return BadRequest(DataResponse.Failure("Invalid privilege id"));
        //         }
        //         else if (ex is NotFoundException)
        //         {
        //             return NotFound(DataResponse.Failure("Privilege not found"));
        //         }
        //         else
        //         {
        //             return InternalServerError(DataResponse.Failure(ex.Message));
        //         }
        //     }
        // }

        // [HttpPost("remove-privilege")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // public async Task<IActionResult> RemovePrivilege([FromBody] PrivilegeDto model)
        // {
        //     try
        //     {
        //         bool success = await _repository.RemovePrivilege(model);
        //         if (success)
        //         {
        //             return Ok(DataResponse.Succeeded("Unmapped privilege successfully"));
        //         }
        //         return NotFound(DataResponse.Failure($"Privilege not found"));
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
        //         {
        //             return BadRequest(DataResponse.Failure("Invalid privilege id"));
        //         }
        //         else if (ex is NotFoundException)
        //         {
        //             return NotFound(DataResponse.Failure("Privilege not found"));
        //         }
        //         else
        //         {
        //             return InternalServerError(DataResponse.Failure(ex.Message));
        //         }
        //     }
        // }
    }
}
