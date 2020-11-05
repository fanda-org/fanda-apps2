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
    public interface IOrgUserRepository :
        ISubRepository<OrgUser, OrgUserDto, OrgUserListDto>
    {
    }

    public class OrgUserRepository : IOrgUserRepository
    {
        public Task<OrgUserDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<OrgUserDto> CreateAsync(Guid superId, OrgUserDto model)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, OrgUserDto model)
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

        public IQueryable<OrgUserListDto> GetAll(Guid superId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrgUserDto>> FindAsync(Guid superId, Expression<Func<OrgUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrgUserDto>> FindAsync(Guid superId, string expression, params object[] args)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Guid superId, Expression<Func<OrgUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public bool Any(Guid superId, string expression, params object[] args)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationErrors> ValidateAsync(Guid superId, OrgUserDto model)
        {
            throw new NotImplementedException();
        }
    }
}
