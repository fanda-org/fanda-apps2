using AutoMapper;
using Fanda.Core.Base;
using Fanda.Domain;
using Fanda.Domain.Context;
using Fanda.Service.Base;
using Fanda.Service.Dto;

namespace Fanda.Service
{
    public interface IPartyCategoryRepository :
        IOrgRepository<PartyCategoryDto>,
        IListRepository<PartyCategoryListDto>
    { }

    public class PartyCategoryRepository :
        OrgRepositoryBase<PartyCategory, PartyCategoryDto, PartyCategoryListDto>, IPartyCategoryRepository
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public PartyCategoryRepository(FandaContext context, IMapper mapper)
            : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //public IQueryable<PartyCategoryListDto> GetAll(Guid orgId)
        //{
        //    if (orgId == null || orgId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("orgId", "Org id is required");
        //    }
        //    IQueryable<PartyCategoryListDto> categories = _context.PartyCategories
        //        .AsNoTracking()
        //        .Where(p => p.OrgId == orgId)
        //        .ProjectTo<PartyCategoryListDto>(_mapper.ConfigurationProvider);

        //    return categories;
        //}

        //public async Task<PartyCategoryDto> GetByIdAsync(Guid id)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("id", "Id is required");
        //    }
        //    PartyCategoryDto category = await _context.PartyCategories
        //        .AsNoTracking()
        //        .ProjectTo<PartyCategoryDto>(_mapper.ConfigurationProvider)
        //        .FirstOrDefaultAsync(pc => pc.Id == id);
        //    if (category == null)
        //    {
        //        throw new NotFoundException("Party category not found");
        //    }
        //    return category;
        //}

        //public async Task<PartyCategoryDto> CreateAsync(Guid orgId, PartyCategoryDto model)
        //{
        //    if (orgId == null || orgId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("orgId", "Org id is required");
        //    }

        //    PartyCategory category = _mapper.Map<PartyCategory>(model);
        //    category.OrgId = orgId;
        //    category.DateCreated = DateTime.UtcNow;
        //    category.DateModified = null;
        //    await _context.PartyCategories.AddAsync(category);
        //    await _context.SaveChangesAsync();
        //    return _mapper.Map<PartyCategoryDto>(category);
        //}

        //public async Task UpdateAsync(Guid id, PartyCategoryDto model)
        //{
        //    if (id != model.Id)
        //    {
        //        throw new ArgumentException("Party category id mismatch");
        //    }

        //    PartyCategory category = _mapper.Map<PartyCategory>(model);
        //    category.DateModified = DateTime.UtcNow;
        //    _context.PartyCategories.Update(category);
        //    await _context.SaveChangesAsync();
        //    //return _mapper.Map<PartyCategoryDto>(category);
        //}

        //public async Task<bool> DeleteAsync(Guid id)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is required");
        //    }

        //    PartyCategory category = await _context.PartyCategories
        //        .FindAsync(id);
        //    if (category != null)
        //    {
        //        _context.PartyCategories.Remove(category);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    throw new KeyNotFoundException("Party category not found");
        //}

        //public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        //{
        //    if (status.Id == null || status.Id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is required");
        //    }

        //    PartyCategory category = await _context.PartyCategories
        //        .FindAsync(status.Id);
        //    if (category != null)
        //    {
        //        category.Active = status.Active;
        //        _context.PartyCategories.Update(category);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    throw new KeyNotFoundException("Party category not found");
        //}

        //public async Task<bool> ExistsAsync(OrgKeyData data) => await _context.ExistsAsync<PartyCategory>(data);

        //public async Task<PartyCategoryDto> GetByAsync(OrgKeyData data)
        //{
        //    var pc = await _context.GetByAsync<PartyCategory>(data);
        //    return _mapper.Map<PartyCategoryDto>(pc);
        //}

        //public async Task<ValidationResultModel> ValidateAsync(Guid orgId, PartyCategoryDto model)
        //{
        //    // Reset validation errors
        //    model.Errors.Clear();

        //    #region Formatting: Cleansing and formatting

        //    model.Code = model.Code.ToUpper();
        //    model.Name = model.Name.TrimExtraSpaces();
        //    model.Description = model.Description.TrimExtraSpaces();

        //    #endregion Formatting: Cleansing and formatting

        //    #region Validation: Duplicate

        //    // Check code duplicate
        //    var duplCode = new OrgKeyData { Field = KeyField.Code, Value = model.Code, Id = model.Id, OrgId = orgId };
        //    if (await ExistsAsync(duplCode))
        //    {
        //        model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
        //    }
        //    // Check name duplicate
        //    var duplName = new OrgKeyData { Field = KeyField.Name, Value = model.Name, Id = model.Id, OrgId = orgId };
        //    if (await ExistsAsync(duplName))
        //    {
        //        model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
        //    }

        //    #endregion Validation: Duplicate

        //    return model.Errors;
        //}
    }
}
