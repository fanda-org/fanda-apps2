using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Accounting.Domain;
using Fanda.Accounting.Domain.Context;
using Fanda.Accounting.Repository.ApiClients;
using Fanda.Accounting.Repository.Dto;
using Fanda.Core;
using Fanda.Core.Base;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Fanda.Accounting.Repository
{
    public interface IOrganizationRepository :
        ISubRepository<Organization, OrganizationDto, OrgListDto>,
        IListRepository<OrgYearListDto>
    {
    }

    public class OrganizationRepository :
        SubRepository<Organization, OrganizationDto, OrgListDto>, IOrganizationRepository
    {
        private readonly IAuthClient _authClient;
        private readonly AcctContext _context;
        private readonly IMapper _mapper;

        public OrganizationRepository(AcctContext context, IMapper mapper, IAuthClient authClient)
            : base(context, mapper, "UserId = @0")
        {
            _context = context;
            _mapper = mapper;
            _authClient = authClient;
        }

        public override async Task<OrganizationDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is required");
            }

            var org = await _context.Organizations
                .AsNoTracking()
                .Include(o => o.OrgContacts).ThenInclude(oc => oc.Contact)
                .Include(o => o.OrgAddresses).ThenInclude(oa => oa.Address)
                //.Where(o => o.Id == id)
                //.ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (org == null)
            {
                throw new NotFoundException("Organization not found");
            }

            return _mapper.Map<OrganizationDto>(org);

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

        //public async Task<IEnumerable<OrganizationDto>> FindAsync(Expression<Func<Organization, bool>> predicate)
        //{
        //    var models = await _context.Organizations
        //        .AsNoTracking()
        //        .Where(predicate)
        //        .ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider)
        //        .ToListAsync();

        //    return models;
        //}

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

        public override async Task<OrganizationDto> CreateAsync(Guid userId, OrganizationDto model)
        {
            var validationResult = await ValidateAsync(userId, model);
            if (!validationResult.IsValid())
            {
                throw new BadRequestException(validationResult);
            }

            var entity = _mapper.Map<Organization>(model);
            entity.DateCreated = DateTime.UtcNow;

            entity.OrgUsers = new List<OrgUser> {new OrgUser {UserId = userId}};
            await _context.Organizations.AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<OrganizationDto>(entity);
        }

        public override async Task UpdateAsync(Guid id, OrganizationDto model)
        {
            if (id != model.Id)
            {
                throw new BadRequestException("Org id mismatch");
            }

            var dbOrg = await _context.Organizations
                .Where(o => o.Id == model.Id)
                .Include(o => o.OrgContacts).ThenInclude(oc => oc.Contact)
                .Include(o => o.OrgAddresses).ThenInclude(oa => oa.Address)
                .Include(o => o.OrgUsers)
                .FirstOrDefaultAsync();

            if (dbOrg == null)
            {
                //org.DateCreated = DateTime.UtcNow;
                //org.DateModified = null;
                //await _context.Organizations.AddAsync(org);
                throw new NotFoundException("Organization not found");
            }

            var userId = Guid.Empty;
            if (dbOrg.OrgUsers != null && dbOrg.OrgUsers.Any())
            {
                userId = dbOrg.OrgUsers.FirstOrDefault().UserId;
            }

            var validationResult = await ValidateAsync(userId, model);
            if (!validationResult.IsValid())
            {
                throw new BadRequestException(validationResult);
            }

            // copy current (incoming) values to db
            var org = _mapper.Map<Organization>(model);
            org.DateCreated = dbOrg.DateCreated;
            org.DateModified = DateTime.UtcNow;
            _context.Entry(dbOrg).CurrentValues.SetValues(org);

            // delete all contacts that are no longer exists
            foreach (var dbOrgContact in dbOrg.OrgContacts)
            {
                var dbContact = dbOrgContact.Contact;
                if (org.OrgContacts.All(oc => oc.Contact.Id != dbContact.Id))
                {
                    _context.Contacts.Remove(dbContact);
                }
            }

            // delete all addresses that are no longer exists
            foreach (var dbOrgAddress in dbOrg.OrgAddresses)
            {
                var dbAddress = dbOrgAddress.Address;
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
                select new {curr, db};
            foreach (var pair in contactPairs)
            {
                if (pair.db == null)
                {
                    var orgContact = new OrgContact
                    {
                        OrgId = org.Id, Organization = org, ContactId = pair.curr.Id, Contact = pair.curr
                    };
                    dbOrg.OrgContacts.Add(orgContact);
                }
                else
                {
                    _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                    //_context.Contacts.Update(pair.db);
                }
            }

            #endregion Contacts

            #region Addresses

            var addressPairs = from curr in org.OrgAddresses.Select(oa => oa.Address)
                join db in dbOrg.OrgAddresses.Select(oa => oa.Address)
                    on curr.Id equals db.Id into grp
                from db in grp.DefaultIfEmpty()
                select new {curr, db};
            foreach (var pair in addressPairs)
            {
                if (pair.db == null)
                {
                    var orgAddress = new OrgAddress
                    {
                        OrgId = org.Id, Organization = org, AddressId = pair.curr.Id, Address = pair.curr
                    };
                    dbOrg.OrgAddresses.Add(orgAddress);
                }
                else
                {
                    _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                    //_context.Addresses.Update(pair.db);
                }
            }

            #endregion Addresses

            //_context.Organizations.Update(dbOrg);
            await _context.SaveChangesAsync();
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is required");
            }

            var org = await _context.Organizations
                .Include(o => o.OrgContacts).ThenInclude(oc => oc.Contact)
                .Include(o => o.OrgAddresses).ThenInclude(oa => oa.Address)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (org != null)
            {
                foreach (var orgContact in org.OrgContacts)
                {
                    _context.Contacts.Remove(orgContact.Contact);
                }

                foreach (var orgAddress in org.OrgAddresses)
                {
                    _context.Addresses.Remove(orgAddress.Address);
                }

                _context.Organizations.Remove(org);
                await _context.SaveChangesAsync();
                return true;
            }

            throw new NotFoundException("Organization not found");
        }

        public override async Task<ValidationErrors> ValidateAsync(Guid userId, OrganizationDto model)
        {
            // Reset validation errors
            model.Errors.Clear();
            if (userId == Guid.Empty)
            {
                model.Errors.AddError(nameof(userId), "User id is required");
                return model.Errors;
            }

            #region Formatting: Cleansing and formatting

            model.Code = model.Code.ToUpper();
            model.Name = model.Name.TrimExtraSpaces();
            model.Description = model.Description.TrimExtraSpaces();

            #endregion Formatting: Cleansing and formatting

            #region UserId validation

            var apiData = await _authClient.GetUserByIdAsync(userId);
            if (apiData == null || apiData.Data == null)
            {
                model.Errors.AddError(nameof(userId), "User not found");
            }

            #endregion UserId validation

            #region Validation: Duplicate

            if (await AnyAsync(userId, GetCodePredicate(model.Code, model.Id)))
            {
                model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }

            if (await AnyAsync(userId, GetNamePredicate(model.Name, model.Id)))
            {
                model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }

            #endregion Validation: Duplicate

            return model.Errors;
        }

        //public virtual async Task<bool> ActivateAsync(Guid id, bool active)
        //{
        //    if (id == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(id), "Id is required");
        //    }
        //    var entity = await _context.Organizations.FindAsync(id);
        //    if (entity == null)
        //    {
        //        throw new NotFoundException("Organization not found");
        //    }
        //    entity.Active = active;
        //    entity.DateModified = DateTime.UtcNow;
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //public virtual async Task<bool> AnyAsync(Expression<Func<Organization, bool>> predicate)
        //{
        //    return await _context.Organizations.AnyAsync(predicate);
        //}

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

            var query = _context.Organizations
                //.Include(o => o.OrgUsers)
                .AsNoTracking()
                .Where(o => o.OrgUsers.Select(ou => ou.UserId).Any(uid => uid == userId))
                .ProjectTo<OrgListDto>(_mapper.ConfigurationProvider);
            return query; //GetAll(query);
        }

        IQueryable<OrgYearListDto> IListRepository<OrgYearListDto>.GetAll(Guid userId)
        {
            // TODO - to be fixed once org-user relationship established
            //if (userId == null || userId == Guid.Empty)
            //{
            //    throw new ArgumentNullException("userId", "User id is required");
            //}

            var query = _context.Organizations
                .Include(o => o.AccountYears)
                //.Include(o => o.OrgUsers)
                .AsNoTracking()
                //.Where(o => o.OrgUsers.Select(ou => ou.UserId).Any(uid => uid == userId))
                .ProjectTo<OrgYearListDto>(_mapper.ConfigurationProvider);
            return query; //GetAll(query);
        }

        //private static IQueryable<T> GetAll<T>(IQueryable<T> query)
        //    where T : BaseListDto
        //{
        //    query = query.Where(c => c.Code != "FANDA");
        //    return query;
        //}

        #endregion List

        #region Privates

        private static ExpressionStarter<Organization> GetCodePredicate(string code, Guid id = default)
        {
            var codeExpression = PredicateBuilder.New<Organization>(e => e.Code == code);
            if (id != Guid.Empty)
            {
                codeExpression = codeExpression.And(e => e.Id != id);
            }

            return codeExpression;
        }

        private static ExpressionStarter<Organization> GetNamePredicate(string name, Guid id = default)
        {
            var nameExpression = PredicateBuilder.New<Organization>(e => e.Name == name);
            if (id != Guid.Empty)
            {
                nameExpression = nameExpression.And(e => e.Id != id);
            }

            return nameExpression;
        }

        #endregion Privates

        #region Implement base abstract class

        protected override void SetSuperId(Guid superId, Organization entity)
        {
            // entity.UserId = superId;
        }

        protected override Guid GetSuperId(Organization entity)
        {
            // return entity.UserId;
            return Guid.Empty;
        }

        protected override Expression<Func<Organization, bool>> GetSuperIdPredicate(Guid superId)
        {
            // return o => o.UserId = superId;
            return o => true;
        }

        #endregion Implement base abstract class
    }
}