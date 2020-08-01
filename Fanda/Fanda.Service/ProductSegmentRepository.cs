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
    public interface IProductSegmentRepository
    {
        Task<List<ProductSegmentDto>> GetAllAsync(Guid orgId, bool? active);
        Task<ProductSegmentDto> GetByIdAsync(Guid segmentId);
        Task<ProductSegmentDto> SaveAsync(Guid orgId, ProductSegmentDto dto);
        Task<bool> DeleteAsync(Guid segmentId);
        string ErrorMessage { get; }
    }

    public class ProductSegmentRepository : IProductSegmentRepository
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public ProductSegmentRepository(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<ProductSegmentDto>> GetAllAsync(Guid orgId, bool? active)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            List<ProductSegmentDto> segments = await _context.ProductSegments
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                .ProjectTo<ProductSegmentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return segments;
        }

        public async Task<ProductSegmentDto> GetByIdAsync(Guid segmentId)
        {
            ProductSegmentDto segment = await _context.ProductSegments
                .ProjectTo<ProductSegmentDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(pc => pc.Id == segmentId);

            if (segment != null)
            {
                return segment;
            }

            throw new KeyNotFoundException("Product segment not found");
        }

        public async Task<ProductSegmentDto> SaveAsync(Guid orgId, ProductSegmentDto dto)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            ProductSegment segment = _mapper.Map<ProductSegment>(dto);
            if (segment.Id == Guid.Empty)
            {
                segment.OrgId = orgId;
                segment.DateCreated = DateTime.UtcNow;
                segment.DateModified = null;
                await _context.ProductSegments.AddAsync(segment);
            }
            else
            {
                segment.DateModified = DateTime.UtcNow;
                _context.ProductSegments.Update(segment);
            }
            await _context.SaveChangesAsync();
            dto = _mapper.Map<ProductSegmentDto>(segment);
            return dto;
        }

        public async Task<bool> DeleteAsync(Guid segmentId)
        {
            ProductSegment segment = await _context.ProductSegments
                .FindAsync(segmentId);
            if (segment != null)
            {
                _context.ProductSegments.Remove(segment);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Product segment not found");
        }
    }
}