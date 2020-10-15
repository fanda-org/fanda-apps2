using Refit;

using System;
using System.Threading.Tasks;

namespace Fanda.Inventory.Repository.ApiClients
{
    public interface IAuthClient
    {
        [Get("/users/{userId}")]
        Task<AuthUser> GetUserAsync(Guid userId);
    }

    public class AuthUser
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Guid TenantId { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        #region additional info

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateLastLogin { get; set; }

        #endregion additional info
    }
}
