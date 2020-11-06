using AutoMapper;
using Fanda.Accounting.Domain;
using Fanda.Accounting.Domain.Context;
using Fanda.Accounting.Repository.Dto;
using Fanda.Core.Base;
using System;
using System.Linq.Expressions;

namespace Fanda.Accounting.Repository
{
    public interface IAccountYearRepository :
        ISubRepository<AccountYear, AccountYearDto, YearListDto>
    {
    }

    public class AccountYearRepository :
        SubRepository<AccountYear, AccountYearDto, YearListDto>, IAccountYearRepository
    {
        //private readonly FandaContext _context;
        //private readonly IMapper _mapper;

        public AccountYearRepository(AcctContext context, IMapper mapper)
            : base(context, mapper)
        {
            //_context = context;
            //_mapper = mapper;
        }

        public override Expression<Func<AccountYear, bool>> GetSuperIdPredicate(Guid? superId)
        {
            return ay => ay.OrgId == superId;
        }

        protected override void SetSuperId(Guid superId, AccountYear entity)
        {
            entity.OrgId = superId;
        }

        protected override Guid GetSuperId(AccountYear entity)
        {
            return entity.OrgId;
        }

        //public IQueryable<YearListDto> GetAll(Guid orgId)
        //{
        //    if (orgId == null || orgId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("orgId", "Org id is required");
        //    }

        //    IQueryable<YearListDto> years = _context.AccountYears
        //        .AsNoTracking()
        //        .Where(p => p.OrgId == orgId)
        //        .ProjectTo<YearListDto>(_mapper.ConfigurationProvider);

        //    return years;
        //}

        //public async Task<AccountYearDto> GetByIdAsync(Guid id/*, bool includeChildren = false*/)
        //{
        //    AccountYearDto year = await _context.AccountYears
        //        .AsNoTracking()
        //        .ProjectTo<AccountYearDto>(_mapper.ConfigurationProvider)
        //        .FirstOrDefaultAsync(pc => pc.Id == id);
        //    if (year != null)
        //    {
        //        return year;
        //    }

        //    throw new KeyNotFoundException("Account year not found");
        //}

        //public async Task<AccountYearDto> CreateAsync(Guid orgId, AccountYearDto model)
        //{
        //    if (orgId == null || orgId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("orgId", "Org id is required");
        //    }

        //    AccountYear year = _mapper.Map<AccountYear>(model);
        //    year.OrgId = orgId;
        //    year.DateCreated = DateTime.UtcNow;
        //    year.DateModified = null;
        //    await _context.AccountYears.AddAsync(year);
        //    await _context.SaveChangesAsync();
        //    return _mapper.Map<AccountYearDto>(year);
        //}

        //public async Task UpdateAsync(Guid id, AccountYearDto model)
        //{
        //    if (id != model.Id)
        //    {
        //        throw new ArgumentException("Year Id mismatch");
        //    }

        //    AccountYear year = _mapper.Map<AccountYear>(model);
        //    year.DateModified = DateTime.UtcNow;
        //    _context.AccountYears.Update(year);
        //    await _context.SaveChangesAsync();
        //    //return _mapper.Map<AccountYearDto>(year);
        //}

        //public async Task<bool> DeleteAsync(Guid id)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("id", "Year id is required");
        //    }

        //    AccountYear year = await _context.AccountYears
        //        .FindAsync(id);
        //    if (year != null)
        //    {
        //        _context.AccountYears.Remove(year);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    throw new KeyNotFoundException("Account Year not found");
        //}

        //public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        //{
        //    if (status.Id == null || status.Id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("id", "Year id is required");
        //    }

        //    AccountYear year = await _context.AccountYears
        //        .FindAsync(status.Id);
        //    if (year != null)
        //    {
        //        year.Active = status.Active;
        //        _context.AccountYears.Update(year);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    throw new KeyNotFoundException("Account year not found");
        //}

        //public async Task<bool> ExistsAsync(OrgKeyData data) => await _context.ExistsAsync<AccountYear>(data);

        //public async Task<AccountYearDto> GetByAsync(OrgKeyData data)
        //{
        //    var year = await _context.GetByAsync<AccountYear>(data);
        //    return _mapper.Map<AccountYearDto>(year);
        //}

        //public Task<ValidationResultModel> ValidateAsync(Guid orgId, AccountYearDto model) => throw new NotImplementedException();
    }
}