using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Accounting.Domain;
using Fanda.Accounting.Domain.Context;
using Fanda.Accounting.Repository.ApiClients;
using Fanda.Accounting.Repository.Dto;
using Fanda.Core;
using Fanda.Core.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fanda.Accounting.Repository
{
    public interface IOrgRoleRepository :
        ISubRepository<OrgUserRole, OrgRoleDto, OrgRoleListDto>
    {
    }

    public class OrgRoleRepository : IOrgRoleRepository
    {
        private readonly AcctContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthClient _authClient;

        public OrgRoleRepository(AcctContext context, IMapper mapper, IAuthClient authClient)
        {
            _context = context;
            _mapper = mapper;
            _authClient = authClient;
        }

        public async Task<OrgRoleDto> GetByIdAsync(Guid roleId)
        {
            if (roleId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(roleId), "Id is required");
            }

            var orgRole = await _context.Set<OrgUserRole>()
                .AsNoTracking()
                .Where(t => t.RoleId == roleId)
                .ProjectTo<OrgUserRole>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            var apiData = await _authClient.GetRoleByIdAsync(roleId);
            if (apiData == null)
            {
                throw new NotFoundException("Role not found");
            }

            return apiData.Data;
        }

        public Task<OrgRoleDto> CreateAsync(Guid superId, OrgRoleDto model)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, OrgRoleDto model)
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

        public IQueryable<OrgRoleListDto> GetAll(Guid superId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Func<OrgUserRole, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrgRoleDto>> FindAsync(Expression<Func<OrgUserRole, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationErrors> ValidateAsync(Guid superId, OrgRoleDto model)
        {
            throw new NotImplementedException();
        }
    }
}
