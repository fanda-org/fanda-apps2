using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Fanda.Accounting.Domain;
using Fanda.Accounting.Domain.Context;
using Fanda.Accounting.Repository.ApiClients;
using Fanda.Accounting.Repository.Dto;
using Fanda.Core;
using Fanda.Core.Base;
using Microsoft.EntityFrameworkCore;

namespace Fanda.Accounting.Repository
{
    public interface IOrgUserRepository :
        ISubRepository<OrgUser, OrgUserDto, OrgUserListDto>
    {
    }

    public class OrgUserRepository : IOrgUserRepository
    {
        private readonly IAuthClient _client;
        private readonly AcctContext _context;
        private readonly IMapper _mapper;

        public OrgUserRepository(AcctContext context, IMapper mapper, IAuthClient client)
        {
            _context = context;
            _mapper = mapper;
            _client = client;
        }

        public Task<OrgUserDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<OrgUserDto> CreateAsync(Guid orgId, OrgUserDto model)
        {
            var tenantId = await _context.Organizations
                .Where(o => o.Id == orgId)
                .Select(o => o.TenantId)
                .FirstOrDefaultAsync();
            if (tenantId == null || tenantId == Guid.Empty)
            {
                throw new NotFoundException("Org not found");
            }

            var res = await _client.CreateUserAsync((Guid)tenantId, model);
            if (res != null && res.Data != null)
            {
                return res.Data;
            }

            throw new BadRequestException(res.ErrorMessage);
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

        public IQueryable<OrgUserListDto> GetAll(Guid? orgId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrgUserDto>> FindAsync(Guid orgId, Expression<Func<OrgUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        //public Task<IEnumerable<OrgUserDto>> FindAsync(Guid orgId, string expression, params object[] args)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<bool> AnyAsync(Guid orgId, Expression<Func<OrgUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        //public bool Any(Guid orgId, string expression, params object[] args)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<ValidationErrors> ValidateAsync(Guid orgId, OrgUserDto model)
        {
            throw new NotImplementedException();
        }

        public Expression<Func<OrgUser, bool>> GetSuperIdPredicate(Guid? superId)
        {
            throw new NotImplementedException();
        }
    }
}