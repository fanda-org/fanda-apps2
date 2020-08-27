using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Core;
using Fanda.Core.Base;
using Fanda.Domain.Base;
using Fanda.Domain.Context;
using Fanda.Service.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service.Base
{
    public abstract class OrgRepositoryBase<TEntity, TModel, TListModel>
        : RepositoryBase<TEntity, TModel, TListModel>
        where TEntity : OrgEntity
        where TModel : BaseDto
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public OrgRepositoryBase(FandaContext context, IMapper mapper)
            : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override IQueryable<TListModel> GetAll(Guid orgId)  // nullable
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(orgId), $"{nameof(orgId)} is required");
            }

            IQueryable<TListModel> qry = _context.Set<TEntity>()
                .AsNoTracking()
                .Where(t => t.OrgId == orgId)
                .ProjectTo<TListModel>(_mapper.ConfigurationProvider);
            return qry;
        }

        public virtual async Task<TModel> CreateAsync(Guid orgId, TModel model)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(orgId), $"{nameof(orgId)} is required");
            }

            var entity = _mapper.Map<TEntity>(model);
            entity.OrgId = orgId;
            entity.DateCreated = DateTime.UtcNow;
            entity.DateModified = null;
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TModel>(entity);
        }

        public virtual async Task<bool> ExistsAsync(OrgKeyData data)
            => await _context.ExistsAsync<TEntity>(data);

        public virtual async Task<TModel> GetByAsync(OrgKeyData data)
        {
            var app = await _context.GetByAsync<TEntity>(data);
            return _mapper.Map<TModel>(app);
        }

        public async Task<ValidationResultModel> ValidateAsync(Guid orgId, TModel model)
        {
            // Reset validation errors
            model.Errors.Clear();

            #region Formatting: Cleansing and formatting

            model.Code = model.Code.TrimExtraSpaces().ToUpper();
            model.Name = model.Name.TrimExtraSpaces();

            #endregion Formatting: Cleansing and formatting

            #region Validation: Duplicate

            // Check email duplicate
            var duplCode = new OrgKeyData { Field = KeyField.Code, Value = model.Code, Id = model.Id, OrgId = orgId };
            if (await ExistsAsync(duplCode))
            {
                model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }
            // Check name duplicate
            var duplName = new OrgKeyData { Field = KeyField.Name, Value = model.Name, Id = model.Id, OrgId = orgId };
            if (await ExistsAsync(duplName))
            {
                model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }

            #endregion Validation: Duplicate

            return model.Errors;
        }
    }
}
