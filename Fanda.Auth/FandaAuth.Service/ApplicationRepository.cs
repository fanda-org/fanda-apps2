using AutoMapper;
using Fanda.Core;
using Fanda.Core.Base;
using FandaAuth.Domain;
using FandaAuth.Service.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FandaAuth.Service
{
    public interface IApplicationRepository :
        IParentRepository<ApplicationDto>,
        IListRepository<ApplicationListDto>
    {
    }

    public class ApplicationRepository :
        RepositoryBase<Application, ApplicationDto, ApplicationListDto>,
        IApplicationRepository
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;

        public ApplicationRepository(AuthContext context, IMapper mapper)
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
        //        throw new ArgumentNullException("id", "Id is missing");
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
        //        throw new ArgumentNullException("Id", "Id is missing");
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

        public async override Task UpdateAsync(Guid id, ApplicationDto model)
        {
            if (id != model.Id)
            {
                throw new BadRequestException("Appication id mismatch");
            }

            Application app = mapper.Map<Application>(model);
            Application dbApp = await context.Applications
                .Where(o => o.Id == app.Id)
                .Include(o => o.AppResources)
                .FirstOrDefaultAsync();

            if (dbApp == null)
            {
                //org.DateCreated = DateTime.UtcNow;
                //org.DateModified = null;
                //await _context.Organizations.AddAsync(org);
                throw new NotFoundException("Application not found");
            }

            try
            {
                // delete all app-resource that are no longer exists
                foreach (AppResource dbAppResource in dbApp.AppResources)
                {
                    //Resource dbResource = dbAppResource.Resource;
                    //if (app.AppResources.All(oc => oc.Resource.Id != dbAppResource.Resource.Id))
                    if (app.AppResources.All(ar => ar.Id != dbAppResource.Id))
                    {
                        //context.Resources.Remove(dbResource);
                        context.Set<AppResource>().Remove(dbAppResource);
                    }
                }
            }
            catch { }

            // copy current (incoming) values to db
            app.DateModified = DateTime.UtcNow;
            context.Entry(dbApp).CurrentValues.SetValues(app);

            #region Resources

            var resourcePairs = from curr in app.AppResources   //.Select(oc => oc.Resource)
                                join db in dbApp.AppResources   //.Select(oc => oc.Resource)
                                     on curr.Id equals db.Id into grp
                                from db in grp.DefaultIfEmpty()
                                select new { curr, db };
            foreach (var pair in resourcePairs)
            {
                if (pair.db != null)
                {
                    context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                    context.Set<AppResource>().Update(pair.db);
                }
                else
                {
                    //var appResource = new AppResource
                    //{
                    //    ApplicationId = app.Id,
                    //    //ResourceId = pair.curr.Id,
                    //};
                    //dbApp.AppResources.Add(pair.curr);
                    context.Set<AppResource>().Add(pair.curr);
                }
            }

            #endregion Resources

            context.Applications.Update(dbApp);
            await context.SaveChangesAsync();
        }

        //public async Task<bool> DeleteAsync(Guid id)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is missing");
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
        //        throw new ArgumentNullException("Id", "Id is missing");
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
