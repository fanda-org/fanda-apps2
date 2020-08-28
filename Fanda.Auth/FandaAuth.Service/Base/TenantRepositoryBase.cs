using AutoMapper;
using Fanda.Core;
using Fanda.Core.Base;
using FandaAuth.Domain;
using FandaAuth.Domain.Base;
using FandaAuth.Service.Extensions;
using System;
using System.Threading.Tasks;

namespace FandaAuth.Service.Base
{
    public abstract class TenantRepositoryBase<TEntity, TModel, TListModel>
        : RootRepositoryBase<TEntity, TModel, TListModel>, ITenantRepository<TModel, TListModel>
        where TEntity : TenantEntity
        where TModel : BaseDto
        where TListModel : BaseListDto
    {
        private readonly AuthContext _context;
        private readonly IMapper _mapper;

        public TenantRepositoryBase(AuthContext context, IMapper mapper)
            : base(context, mapper, "TenantId == '{0}'")
        {
            _context = context;
            _mapper = mapper;
        }

        //public virtual IQueryable<TListModel> GetAll(Guid tenantId)
        //{
        //    if (tenantId == null || tenantId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(tenantId), $"{nameof(tenantId)} is required");
        //    }

        //    IQueryable<TListModel> qry = _context.Set<TEntity>()
        //        .AsNoTracking()
        //        .Where(t => t.TenantId == tenantId)
        //        .ProjectTo<TListModel>(_mapper.ConfigurationProvider);
        //    return qry;
        //}

        public virtual async Task<TModel> CreateAsync(Guid tenantId, TModel model)
        {
            if (tenantId == null || tenantId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(tenantId), "TenantId is required");
            }

            var entity = _mapper.Map<TEntity>(model);
            entity.TenantId = tenantId;
            entity.DateCreated = DateTime.UtcNow;
            entity.DateModified = null;
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TModel>(entity);
        }

        public virtual async Task UpdateAsync(Guid id, TModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("Id mismatch");
            }
            var entity = _mapper.Map<TEntity>(model);
            entity.DateModified = DateTime.UtcNow;
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> ExistsAsync(TenantKeyData data)
            => await _context.ExistsAsync<TEntity>(data);

        public virtual async Task<TModel> GetByAsync(TenantKeyData data)
        {
            var app = await _context.GetByAsync<TEntity>(data);
            return _mapper.Map<TModel>(app);
        }

        public async Task<ValidationResultModel> ValidateAsync(Guid tenantId, TModel model)
        {
            // Reset validation errors
            model.Errors.Clear();

            #region Formatting: Cleansing and formatting

            model.Code = model.Code.TrimExtraSpaces().ToUpper();
            model.Name = model.Name.TrimExtraSpaces();

            #endregion Formatting: Cleansing and formatting

            #region Validation: Duplicate

            // Check email duplicate
            var duplCode = new TenantKeyData { Field = KeyField.Code, Value = model.Code, Id = model.Id, TenantId = tenantId };
            if (await ExistsAsync(duplCode))
            {
                model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }
            // Check name duplicate
            var duplName = new TenantKeyData { Field = KeyField.Name, Value = model.Name, Id = model.Id, TenantId = tenantId };
            if (await ExistsAsync(duplName))
            {
                model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }

            #endregion Validation: Duplicate

            return model.Errors;
        }
    }
}
