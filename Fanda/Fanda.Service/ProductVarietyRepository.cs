using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Core.Models;
using Fanda.Domain;
using Fanda.Domain.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Infrastructure
{
    public interface IProductVarietyRepository
    {
        Task<List<ProductVarietyDto>> GetAllAsync(Guid orgId, bool? active);
        Task<ProductVarietyDto> GetByIdAsync(Guid varietyId);
        Task<ProductVarietyDto> SaveAsync(Guid orgId, ProductVarietyDto dto);
        Task<bool> DeleteAsync(Guid varietyId);
        string ErrorMessage { get; }
    }

    public class ProductVarietyRepository : IProductVarietyRepository
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public ProductVarietyRepository(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<ProductVarietyDto>> GetAllAsync(Guid orgId, bool? active)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            List<ProductVarietyDto> varieties = await _context.ProductVarieties
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .ProjectTo<ProductVarietyDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return varieties;
        }

        public async Task<ProductVarietyDto> GetByIdAsync(Guid varietyId)
        {
            ProductVarietyDto variety = await _context.ProductVarieties
                .ProjectTo<ProductVarietyDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(pc => pc.Id == varietyId);

            if (variety != null)
            {
                return variety;
            }

            throw new KeyNotFoundException("Product variety not found");
        }

        public async Task<ProductVarietyDto> SaveAsync(Guid orgId, ProductVarietyDto dto)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            ProductVariety variety = _mapper.Map<ProductVariety>(dto);
            if (variety.Id == Guid.Empty)
            {
                variety.OrgId = orgId;
                variety.DateCreated = DateTime.UtcNow;
                variety.DateModified = null;
                await _context.ProductVarieties.AddAsync(variety);
            }
            else
            {
                variety.DateModified = DateTime.UtcNow;
                _context.ProductVarieties.Update(variety);
            }
            await _context.SaveChangesAsync();
            dto = _mapper.Map<ProductVarietyDto>(variety);
            return dto;
        }

        public async Task<bool> DeleteAsync(Guid varietyId)
        {
            ProductVariety variety = await _context.ProductVarieties
                .FindAsync(varietyId);
            if (variety != null)
            {
                _context.ProductVarieties.Remove(variety);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Product variety not found");
        }
    }
}