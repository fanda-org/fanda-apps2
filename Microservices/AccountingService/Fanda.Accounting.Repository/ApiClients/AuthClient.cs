using Fanda.Accounting.Repository.Dto;
using Fanda.Core.Base;
using Refit;
using System;
using System.Threading.Tasks;

namespace Fanda.Accounting.Repository.ApiClients
{
    public interface IAuthClient
    {
        #region Users

        [Get("/users/{userId}")]
        Task<DataResponse<OrgUserDto>> GetUserByIdAsync(Guid userId);

        [Get("/users?tenantId={tenantId}")]
        Task<DataResponse<OrgUserDto>> GetAllUsersAsync(Guid tenantId);

        [Post("/users/{tenantId}")]
        Task<DataResponse<OrgUserDto>> CreateUserAsync(Guid tenantId, OrgUserDto dto);

        #endregion Users

        #region Roles

        [Get("/roles/{roleID}")]
        Task<DataResponse<OrgRoleDto>> GetRoleByIdAsync(Guid roleId);

        #endregion Roles
    }

    //public class AuthUser
    //{
    //    public Guid Id { get; set; }
    //    public string UserName { get; set; }
    //    public string Email { get; set; }
    //    public Guid TenantId { get; set; }
    //    public bool Active { get; set; }
    //    public DateTime DateCreated { get; set; }
    //    public DateTime? DateModified { get; set; }

    //    #region additional info

    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public DateTime? DateLastLogin { get; set; }

    //    #endregion additional info
    //}
}
