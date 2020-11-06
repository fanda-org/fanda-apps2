using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fanda.Authentication.Domain;
using Fanda.Authentication.Repository.Dto;
using Fanda.Core;
using Fanda.Core.Base;
using Microsoft.EntityFrameworkCore;

namespace Fanda.Authentication.Repository
{
    public interface IApplicationRepository :
        //IRepositoryBase<ApplicationDto, ApplicationListDto, KeyData>
        ISuperRepository<Application, ApplicationDto, ApplicationListDto>
    {
    }

    public class ApplicationRepository :
        //RepositoryBase<Application, ApplicationDto, ApplicationListDto, KeyData>,
        SuperRepository<Application, ApplicationDto, ApplicationListDto>,
        IApplicationRepository
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;

        public ApplicationRepository(AuthContext context, IMapper mapper)
            //: base(context, mapper, string.Empty)
            : base(context, mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        //public IQueryable<ApplicationListDto> GetAll(Guid parentId)  // nullable
        //{
        //    IQueryable<ApplicationListDto> qry = context.Applications
        //        .AsNoTracking()
        //        .ProjectTo<ApplicationListDto>(mapper.ConfigurationProvider);
        //    return qry;
        //}

        //public async Task<ApplicationDto> GetByIdAsync(Guid id/*, bool includeChildren = false*/)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("id", "Id is required");
        //    }

        //    var app = await context.Applications
        //        .AsNoTracking()
        //        .ProjectTo<ApplicationDto>(mapper.ConfigurationProvider)
        //        .FirstOrDefaultAsync(t => t.Id == id);
        //    //var app = mapper.Map<ApplicationDto>(appBase);

        //    if (app == null)
        //    {
        //        throw new NotFoundException("Application not found");
        //    }
        //    //else if (!includeChildren)
        //    //{
        //    return app;
        //    //}

        //    //app.AppResources = await context.Set<AppResource>()
        //    //    .AsNoTracking()
        //    //    .Where(m => m.ApplicationId == id)
        //    //    //.SelectMany(oc => oc.AppResources.Select(c => c.Resource))
        //    //    .ProjectTo<AppResourceDto>(mapper.ConfigurationProvider)
        //    //    .ToListAsync();

        //    //return app;
        //}

        //public async Task<AppChildrenDto> GetChildrenByIdAsync(Guid id)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is required");
        //    }

        //    var appResources = new AppChildrenDto
        //    {
        //        AppResources = await context.Set<AppResource>()
        //            .AsNoTracking()
        //            .Where(m => m.ApplicationId == id)
        //            //.SelectMany(oc => oc.AppResources.Select(c => c.Resource))
        //            .ProjectTo<AppResourceDto>(mapper.ConfigurationProvider)
        //            .ToListAsync()
        //    };
        //    return appResources;
        //}

        //public async Task<ApplicationDto> CreateAsync(ApplicationDto model)
        //{
        //    var app = mapper.Map<Application>(model);
        //    app.DateCreated = DateTime.UtcNow;
        //    app.DateModified = null;
        //    // app.Active = true;
        //    //foreach (var ar in app.AppResources)
        //    //{
        //    //    ar.DateCreated = DateTime.UtcNow;
        //    //    ar.DateModified = null;
        //    //}
        //    await context.Applications.AddAsync(app);
        //    await context.SaveChangesAsync();
        //    return mapper.Map<ApplicationDto>(app);
        //}

        public override async Task UpdateAsync(Guid id, ApplicationDto model)
        {
            if (id != model.Id)
            {
                throw new BadRequestException("Application id mismatch");
            }

            var dbApp = await context.Applications
                .Include(a => a.AppResources)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
            if (dbApp == null)
            {
                throw new NotFoundException("Application not found");
            }

            // copy current (incoming) values to db
            var app = mapper.Map<Application>(model);
            app.DateModified = DateTime.UtcNow;
            context.Entry(dbApp).CurrentValues.SetValues(app);

            try
            {
                // delete all app-resource that are no longer exists
                foreach (var dbAppResource in dbApp.AppResources)
                {
                    if (app.AppResources.All(ar => ar.Id != dbAppResource.Id))
                    {
                        context.Set<AppResource>().Remove(dbAppResource);
                    }
                }
            }
            catch { }

            #region Resources

            var resourcePairs = from curr in app.AppResources
                join db in dbApp.AppResources
                    on curr.Id equals db.Id into grp
                from db in grp.DefaultIfEmpty()
                select new {curr, db};
            foreach (var pair in resourcePairs)
            {
                if (pair.db != null)
                {
                    context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                    // context.Set<AppResource>().Update(pair.db);
                }
                else
                {
                    context.Set<AppResource>().Add(pair.curr);
                }
            }

            #endregion Resources

            // context.Applications.Update(dbApp);
            await context.SaveChangesAsync();
        }

        public override async Task<ValidationErrors> ValidateAsync(ApplicationDto model)
        {
            await base.ValidateAsync(model);

            #region Validate AppResources - uniqueness of code & name (find duplicates in a list)

            bool duplicateCodeFound = model.AppResources
                .GroupBy(x => x.Code)
                .Any(g => g.Count() > 1);
            if (duplicateCodeFound)
            {
                model.Errors.AddError("AppResouce.Code", "App resource code already exists");
            }

            bool duplicateNameFound = model.AppResources
                .GroupBy(x => x.Name)
                .Any(g => g.Count() > 1);
            if (duplicateNameFound)
            {
                model.Errors.AddError("AppResouce.Name", "App resource name already exists");
            }

            #endregion Validate AppResources - uniqueness of code & name (find duplicates in a list)

            return model.Errors;
        }

        //protected override Guid GetParentId(Application entity)
        //{
        //    return Guid.Empty;
        //}

        //protected override void SetParentId(KeyData keyData, Guid parentId)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override void SetParentId(Application entity, Guid parentId)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<bool> DeleteAsync(Guid id)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is required");
        //    }
        //    var app = await context.Applications
        //        .FindAsync(id);
        //    if (app == null)
        //    {
        //        throw new NotFoundException("Application not found");
        //    }

        //    context.Applications.Remove(app);
        //    await context.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        //{
        //    if (status.Id == null || status.Id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is required");
        //    }

        //    var app = await context.Applications
        //        .FindAsync(status.Id);
        //    if (app != null)
        //    {
        //        app.Active = status.Active;
        //        app.DateModified = DateTime.UtcNow;
        //        context.Applications.Update(app);
        //        await context.SaveChangesAsync();
        //        return true;
        //    }
        //    throw new NotFoundException("Application not found");
        //}

        //public async Task<bool> ExistsAsync(KeyData data)
        //    => await context.ExistsAsync<Application>(data);

        //public async Task<ApplicationDto> GetByAsync(KeyData data)
        //{
        //    var app = await context.GetByAsync<Application>(data);
        //    return mapper.Map<ApplicationDto>(app);
        //}

        //public async Task<ValidationResultModel> ValidateAsync(ApplicationDto model)
        //{
        //    // Reset validation errors
        //    model.Errors.Clear();

        //    #region Formatting: Cleansing and formatting

        //    model.Code = model.Code.TrimExtraSpaces().ToUpper();
        //    model.Name = model.Name.TrimExtraSpaces();

        //    #endregion Formatting: Cleansing and formatting

        //    #region Validation: Duplicate

        //    // Check email duplicate
        //    var duplCode = new KeyData { Field = KeyField.Code, Value = model.Code, Id = model.Id };
        //    if (await ExistsAsync(duplCode))
        //    {
        //        model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
        //    }
        //    // Check name duplicate
        //    var duplName = new KeyData { Field = KeyField.Name, Value = model.Name, Id = model.Id };
        //    if (await ExistsAsync(duplName))
        //    {
        //        model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
        //    }

        //    #endregion Validation: Duplicate

        //    return model.Errors;
        //}

        // public async Task<bool> MapResource(AppResourceDto model)
        // {
        //     var appResource = mapper.Map<AppResource>(model);
        //     await context.Set<AppResource>().AddAsync(appResource);
        //     await context.SaveChangesAsync();
        //     return true;
        // }

        // public async Task<bool> UnmapResource(AppResourceDto model)
        // {
        //     var appResource = await context.Set<AppResource>()
        //         .FirstOrDefaultAsync(ar => ar.ApplicationId == model.ApplicationId &&
        //             ar.ResourceId == model.ResourceId);
        //     if (appResource == null)
        //     {
        //         throw new NotFoundException("App Resource not found");
        //     }
        //     context.Set<AppResource>().Remove(appResource);
        //     await context.SaveChangesAsync();
        //     return true;
        // }
    }
}