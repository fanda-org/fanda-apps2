﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Core.Models;
using Fanda.Core.Base;
using Fanda.Domain;
using Fanda.Domain.Context;
using Fanda.Infrastructure.Base;
using Fanda.Infrastructure.Extensions;
using Fanda.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Infrastructure
{
    public interface IUnitRepository :
        IRepository<UnitDto>,
        IListRepository<UnitListDto>
    { }

    public class UnitRepository : IUnitRepository
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public UnitRepository(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IQueryable<UnitListDto> GetAll(Guid orgId)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }
            IQueryable<UnitListDto> units = _context.Units
                .AsNoTracking()
                .Where(p => p.OrgId == orgId)
                .ProjectTo<UnitListDto>(_mapper.ConfigurationProvider);

            return units;
        }

        public async Task<UnitDto> GetByIdAsync(Guid id, bool includeChildren = false)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is missing");
            }
            var unit = await _context.Units
                .AsNoTracking()
                .ProjectTo<UnitDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(pc => pc.Id == id);
            if (unit != null)
            {
                return unit;
            }
            throw new KeyNotFoundException("Unit not found");
        }

        public async Task<UnitDto> CreateAsync(Guid orgId, UnitDto model)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            var unit = _mapper.Map<Unit>(model);
            unit.OrgId = orgId;
            unit.DateCreated = DateTime.UtcNow;
            unit.DateModified = null;
            await _context.Units.AddAsync(unit);
            await _context.SaveChangesAsync();
            return _mapper.Map<UnitDto>(unit);
        }

        public async Task UpdateAsync(Guid id, UnitDto model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("Unit id mismatch");
            }

            var unit = _mapper.Map<Unit>(model);
            unit.DateModified = DateTime.UtcNow;
            _context.Units.Update(unit);
            await _context.SaveChangesAsync();
            //return _mapper.Map<UnitDto>(unit);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            var unit = await _context.Units
                .FindAsync(id);
            if (unit != null)
            {
                _context.Units.Remove(unit);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Unit not found");
        }

        public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            var unit = await _context.Units
                .FindAsync(status.Id);
            if (unit != null)
            {
                unit.Active = status.Active;
                _context.Units.Update(unit);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Unit not found");
        }

        public async Task<bool> ExistsAsync(Duplicate data) => await _context.ExistsAsync<Unit>(data);

        public async Task<ValidationResultModel> ValidateAsync(Guid orgId, UnitDto model)
        {
            // Reset validation errors
            model.Errors.Clear();

            #region Formatting: Cleansing and formatting
            model.Code = model.Code.ToUpper();
            model.Name = model.Name.TrimExtraSpaces();
            model.Description = model.Description.TrimExtraSpaces();
            #endregion

            #region Validation: Duplicate
            // Check code duplicate
            var duplCode = new Duplicate { Field = DuplicateField.Code, Value = model.Code, Id = model.Id, ParentId = orgId };
            if (await ExistsAsync(duplCode))
            {
                model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }
            // Check name duplicate
            var duplName = new Duplicate { Field = DuplicateField.Name, Value = model.Name, Id = model.Id, ParentId = orgId };
            if (await ExistsAsync(duplName))
            {
                model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }
            #endregion

            return model.Errors;
        }
    }
}