using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Core.Models;
using Fanda.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Infrastructure
{
    public interface IAccountYearRepository :
        IRepository<AccountYearDto>,
        IListRepository<YearListDto>
    { }

    public class AccountYearRepository : IAccountYearRepository
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public AccountYearRepository(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IQueryable<YearListDto> GetAll(Guid orgId)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            IQueryable<YearListDto> years = _context.AccountYears
                .AsNoTracking()
                .Where(p => p.OrgId == orgId)
                .ProjectTo<YearListDto>(_mapper.ConfigurationProvider);

            return years;
        }

        public async Task<AccountYearDto> GetByIdAsync(Guid id, bool includeChildren = false)
        {
            AccountYearDto year = await _context.AccountYears
                .AsNoTracking()
                .ProjectTo<AccountYearDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(pc => pc.Id == id);
            if (year != null)
            {
                return year;
            }

            throw new KeyNotFoundException("Account year not found");
        }

        public async Task<AccountYearDto> CreateAsync(Guid orgId, AccountYearDto model)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            AccountYear year = _mapper.Map<AccountYear>(model);
            year.OrgId = orgId;
            year.DateCreated = DateTime.UtcNow;
            year.DateModified = null;
            await _context.AccountYears.AddAsync(year);
            await _context.SaveChangesAsync();
            return _mapper.Map<AccountYearDto>(year);
        }

        public async Task UpdateAsync(Guid id, AccountYearDto model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("Year Id mismatch");
            }

            AccountYear year = _mapper.Map<AccountYear>(model);
            year.DateModified = DateTime.UtcNow;
            _context.AccountYears.Update(year);
            await _context.SaveChangesAsync();
            //return _mapper.Map<AccountYearDto>(year);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Year id is missing");
            }

            AccountYear year = await _context.AccountYears
                .FindAsync(id);
            if (year != null)
            {
                _context.AccountYears.Remove(year);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Account Year not found");
        }

        public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Year id is missing");
            }

            AccountYear year = await _context.AccountYears
                .FindAsync(status.Id);
            if (year != null)
            {
                year.Active = status.Active;
                _context.AccountYears.Update(year);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Account year not found");
        }

        public Task<bool> ExistsAsync(Duplicate data) => _context.ExistsAsync<AccountYear>(data);

        public Task<ValidationResultModel> ValidateAsync(Guid orgId, AccountYearDto model) => throw new NotImplementedException();
    }
}