using Fanda.Accounting.Domain;
using Fanda.Accounting.Repository.Dto;
using Fanda.Core;
using Fanda.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fanda.Accounting.Repository
{
    public interface IUserRoleRepository :
        ISubRepository<OrgUserRole, UserRoleDto, UserRoleListDto>
    {
    }

    public class UserRoleRepository : IUserRoleRepository
    {
        public Task<UserRoleDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserRoleDto> CreateAsync(Guid superId, UserRoleDto model)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, UserRoleDto model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ActivateAsync(Guid id, bool active)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserRoleListDto> GetAll(Guid superId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Func<OrgUserRole, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserRoleDto>> FindAsync(Expression<Func<OrgUserRole, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationErrors> ValidateAsync(Guid superId, UserRoleDto model)
        {
            throw new NotImplementedException();
        }
    }
}
