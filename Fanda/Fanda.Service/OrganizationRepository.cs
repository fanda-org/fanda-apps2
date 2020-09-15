using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Core;
using Fanda.Core.Base;
using Fanda.Domain;
using Fanda.Domain.Context;
using Fanda.Service.ApiClients;
using Fanda.Service.Dto;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fanda.Service
{
    public interface IOrganizationRepository :
        ISubRepository<Organization, OrganizationDto, OrgListDto>,
        IListRepository<OrgYearListDto>
    {
    }

    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthClient _authClient;

        public OrganizationRepository(FandaContext context, IMapper mapper, IAuthClient authClient)
        {
            _context = context;
            _mapper = mapper;
            _authClient = authClient;
        }

        public async Task<OrganizationDto> GetByIdAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is required");
            }

            OrganizationDto org = await _context.Organizations
                .AsNoTracking()
                .ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (org == null)
            {
                throw new NotFoundException("Organization not found");
            }
            return org;

            #region commented

            //org.Contacts = await _context.Organizations
            //    .AsNoTracking()
            //    .Where(m => m.Id == id)
            //    .SelectMany(oc => oc.OrgContacts.Select(c => c.Contact))
            //    .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
            //    .ToListAsync();
            //org.Addresses = await _context.Organizations
            //    .AsNoTracking()
            //    .Where(m => m.Id == id)
            //    .SelectMany(oa => oa.OrgAddresses.Select(a => a.Address))
            //    .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
            //    .ToListAsync();
            //return org;

            #endregion commented
        }

        public async Task<IEnumerable<OrganizationDto>> FindAsync(Expression<Func<Organization, bool>> predicate)
        {
            var models = await _context.Organizations
                .AsNoTracking()
                .Where(predicate)
                .ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return models;
        }

        //public async Task<OrgChildrenDto> GetChildrenByIdAsync(Guid id)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is required");
        //    }

        //    var org = new OrgChildrenDto
        //    {
        //        Contacts = await _context.Organizations
        //            .AsNoTracking()
        //            .Where(m => m.Id == id)
        //            .SelectMany(oc => oc.OrgContacts.Select(c => c.Contact))
        //            .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
        //            .ToListAsync(),
        //        Addresses = await _context.Organizations
        //            .AsNoTracking()
        //            .Where(m => m.Id == id)
        //            .SelectMany(oa => oa.OrgAddresses.Select(a => a.Address))
        //            .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
        //            .ToListAsync()
        //    };
        //    return org;
        //}

        //public async Task<OrganizationDto> CreateAsync(OrganizationDto model)
        //{
        //    Organization org = _mapper.Map<Organization>(model);
        //    org.DateCreated = DateTime.UtcNow;
        //    org.DateModified = null;
        //    await _context.Organizations.AddAsync(org);
        //    await _context.SaveChangesAsync();
        //    model = _mapper.Map<OrganizationDto>(org);
        //    return model;
        //}

        public virtual async Task<OrganizationDto> CreateAsync(Guid userId, OrganizationDto model)
        {
            var validationResult = await ValidateAsync(userId, model);
            if (!validationResult.IsValid())
            {
                throw new BadRequestException(validationResult);
            }
            var entity = _mapper.Map<Organization>(model);
            entity.DateCreated = DateTime.UtcNow;
            await _context.Organizations.AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<OrganizationDto>(entity);
        }

        public async Task UpdateAsync(Guid id, OrganizationDto model)
        {
            if (id != model.Id)
            {
                throw new BadRequestException("Org id mismatch");
            }

            Organization dbOrg = await _context.Organizations
                .Where(o => o.Id == model.Id)
                .Include(o => o.OrgContacts).ThenInclude(oc => oc.Contact)
                .Include(o => o.OrgAddresses).ThenInclude(oa => oa.Address)
                .FirstOrDefaultAsync();

            if (dbOrg == null)
            {
                //org.DateCreated = DateTime.UtcNow;
                //org.DateModified = null;
                //await _context.Organizations.AddAsync(org);
                throw new NotFoundException("Organization not found");
            }

            var validationResult = await ValidateAsync(/*dbOrg.UserId or */ Guid.Empty, model);
            if (!validationResult.IsValid())
            {
                throw new BadRequestException(validationResult);
            }

            // copy current (incoming) values to db
            Organization org = _mapper.Map<Organization>(model);
            org.DateModified = DateTime.UtcNow;
            _context.Entry(dbOrg).CurrentValues.SetValues(org);

            // delete all contacts that are no longer exists
            foreach (OrgContact dbOrgContact in dbOrg.OrgContacts)
            {
                Contact dbContact = dbOrgContact.Contact;
                if (org.OrgContacts.All(oc => oc.Contact.Id != dbContact.Id))
                {
                    _context.Contacts.Remove(dbContact);
                }
            }
            // delete all addresses that are no longer exists
            foreach (OrgAddress dbOrgAddress in dbOrg.OrgAddresses)
            {
                Address dbAddress = dbOrgAddress.Address;
                if (org.OrgAddresses.All(oa => oa.Address.Id != dbAddress.Id))
                {
                    _context.Addresses.Remove(dbAddress);
                }
            }

            #region Contacts

            var contactPairs = from curr in org.OrgContacts.Select(oc => oc.Contact)
                               join db in dbOrg.OrgContacts.Select(oc => oc.Contact)
                                    on curr.Id equals db.Id into grp
                               from db in grp.DefaultIfEmpty()
                               select new { curr, db };
            foreach (var pair in contactPairs)
            {
                if (pair.db == null)
                {
                    var orgContact = new OrgContact
                    {
                        OrgId = org.Id,
                        Organization = org,
                        ContactId = pair.curr.Id,
                        Contact = pair.curr
                    };
                    dbOrg.OrgContacts.Add(orgContact);
                }
                else
                {
                    _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                    _context.Contacts.Update(pair.db);
                }
            }

            #endregion Contacts

            #region Addresses

            var addressPairs = from curr in org.OrgAddresses.Select(oa => oa.Address)
                               join db in dbOrg.OrgAddresses.Select(oa => oa.Address)
                                    on curr.Id equals db.Id into grp
                               from db in grp.DefaultIfEmpty()
                               select new { curr, db };
            foreach (var pair in addressPairs)
            {
                if (pair.db == null)
                {
                    var orgAddress = new OrgAddress
                    {
                        OrgId = org.Id,
                        Organization = org,
                        AddressId = pair.curr.Id,
                        Address = pair.curr
                    };
                    dbOrg.OrgAddresses.Add(orgAddress);
                }
                else
                {
                    _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                    _context.Addresses.Update(pair.db);
                }
            }

            #endregion Addresses

            _context.Organizations.Update(dbOrg);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is required");
            }

            Organization org = await _context.Organizations
                .Include(o => o.OrgContacts).ThenInclude(oc => oc.Contact)
                .Include(o => o.OrgAddresses).ThenInclude(oa => oa.Address)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (org != null)
            {
                foreach (OrgContact orgContact in org.OrgContacts)
                {
                    _context.Contacts.Remove(orgContact.Contact);
                }
                foreach (OrgAddress orgAddress in org.OrgAddresses)
                {
                    _context.Addresses.Remove(orgAddress.Address);
                }
                _context.Organizations.Remove(org);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new NotFoundException("Organization not found");
        }

        public virtual async Task<bool> ActivateAsync(Guid id, bool active)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is required");
            }
            var entity = await _context.Organizations.FindAsync(id);
            if (entity == null)
            {
                throw new NotFoundException("Organization not found");
            }
            entity.Active = active;
            entity.DateModified = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<Organization, bool>> predicate)
        {
            return await _context.Organizations.AnyAsync(predicate);
        }

        //public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        //{
        //    if (status.Id == null || status.Id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException("Id", "Id is required");
        //    }

        //    Organization org = await _context.Organizations
        //        .FindAsync(status.Id);
        //    if (org != null)
        //    {
        //        org.Active = status.Active;
        //        _context.Organizations.Update(org);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    throw new NotFoundException("Organization not found");
        //}

        // public Task<bool> ExistsAsync(KeyData data) => _context.ExistsAsync<Organization>(data);

        //public async Task<OrganizationDto> GetByAsync(KeyData data)
        //{
        //    var org = await _context.GetByAsync<Organization>(data);
        //    return _mapper.Map<OrganizationDto>(org);
        //}

        #region List

        IQueryable<OrgListDto> IListRepository<OrgListDto>.GetAll(Guid userId)
        {
            //if (userId == null || userId == Guid.Empty)
            //{
            //    throw new ArgumentNullException("userId", "User id is required");
            //}

            IQueryable<OrgListDto> query = _context.Organizations
                //.Include(o => o.OrgUsers)
                .AsNoTracking()
                //.Where(o => o.OrgUsers.Select(ou => ou.UserId).Any(uid => uid == userId))
                .ProjectTo<OrgListDto>(_mapper.ConfigurationProvider);
            return GetAll(query);
        }

        IQueryable<OrgYearListDto> IListRepository<OrgYearListDto>.GetAll(Guid userId)
        {
            // TODO - to be fixed once org-user relationship established
            //if (userId == null || userId == Guid.Empty)
            //{
            //    throw new ArgumentNullException("userId", "User id is required");
            //}

            IQueryable<OrgYearListDto> query = _context.Organizations
                .Include(o => o.AccountYears)
                //.Include(o => o.OrgUsers)
                .AsNoTracking()
                //.Where(o => o.OrgUsers.Select(ou => ou.UserId).Any(uid => uid == userId))
                .ProjectTo<OrgYearListDto>(_mapper.ConfigurationProvider);
            return GetAll(query);
        }

        private IQueryable<T> GetAll<T>(IQueryable<T> query)
            where T : BaseListDto
        {
            query = query.Where(c => c.Code != "FANDA");
            return query;
        }

        #endregion List

        public async Task<ValidationErrors> ValidateAsync(Guid userId, OrganizationDto model)
        {
            // Reset validation errors
            model.Errors.Clear();

            #region Formatting: Cleansing and formatting

            model.Code = model.Code.ToUpper();
            model.Name = model.Name.TrimExtraSpaces();
            model.Description = model.Description.TrimExtraSpaces();

            #endregion Formatting: Cleansing and formatting

            #region UserId validation

            if (userId == null || userId == Guid.Empty)
            {
                model.Errors.AddError(nameof(userId), "User id is required");
            }
            var user = await _authClient.GetUserAsync(userId);
            if (user == null)
            {
                model.Errors.AddError(nameof(userId), "User not found");
            }

            #endregion UserId validation

            #region Validation: Duplicate

            if (await AnyAsync(GetCodePredicate(model.Code, model.Id)))
            {
                model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }
            if (await AnyAsync(GetNamePredicate(model.Name, model.Id)))
            {
                model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }

            #endregion Validation: Duplicate

            return model.Errors;
        }

        #region Privates

        private ExpressionStarter<Organization> GetCodePredicate(string code, Guid id = default)
        {
            var codeExpression = PredicateBuilder.New<Organization>(e => e.Code == code);
            if (id != null && id != Guid.Empty)
            {
                codeExpression = codeExpression.And(e => e.Id != id);
            }
            return codeExpression;
        }

        private ExpressionStarter<Organization> GetNamePredicate(string name, Guid id = default)
        {
            var nameExpression = PredicateBuilder.New<Organization>(e => e.Name == name);
            if (id != null && id != Guid.Empty)
            {
                nameExpression = nameExpression.And(e => e.Id != id);
            }
            return nameExpression;
        }

        #endregion Privates
    }
}
