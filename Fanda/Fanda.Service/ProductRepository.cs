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
    public interface IProductRepository
    {
        Task<List<ProductDto>> GetAllAsync(Guid orgId, bool? active);
        Task<ProductDto> GetByIdAsync(Guid productId);
        Task<ProductDto> SaveAsync(Guid orgId, ProductDto dto);
        Task<bool> DeleteAsync(Guid productId);
        string ErrorMessage { get; }
    }

    public class ProductRepository : IProductRepository
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<ProductDto>> GetAllAsync(Guid orgId, bool? active)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            List<Product> products = await _context.Products
                .Where(p => p.OrgId == p.OrgId)
                .Where(p => p.Active == ((active == null) ? p.Active : active))
                .AsNoTracking()
                //.ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto> GetByIdAsync(Guid productId)
        {
            ProductDto product = await _context.Products
                //.Include(p => p.ProductIngredients)
                //.Include(p => p.ProductPricings).ThenInclude(pr => pr.PricingRanges)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(pc => pc.Id == productId);

            if (product != null)
            {
                return product; //_mapper.Map<ProductViewModel>(product);
            }

            throw new KeyNotFoundException("Product not found");
        }

        public async Task<ProductDto> SaveAsync(Guid orgId, ProductDto dto)
        {
            if (orgId == null || orgId == Guid.Empty)
            {
                throw new ArgumentNullException("orgId", "Org id is missing");
            }

            Product product = _mapper.Map<Product>(dto);
            if (product.Id == Guid.Empty)
            {
                product.OrgId = orgId;
                product.DateCreated = DateTime.UtcNow;
                product.DateModified = null;
                await _context.Products.AddAsync(product);
            }
            else
            {
                Product dbProd = await _context.Products
                    .Where(p => p.Id == product.Id)
                    .Include(p => p.ParentIngredients)
                    .Include(p => p.ProductPricings).ThenInclude(pr => pr.PricingRanges)
                    .SingleOrDefaultAsync();
                if (dbProd == null)
                {
                    product.DateCreated = DateTime.UtcNow;
                    product.DateModified = null;
                    await _context.Products.AddAsync(product);
                }
                else
                {
                    product.DateModified = DateTime.UtcNow;
                    // delete all ingredients that no longer exists
                    foreach (ProductIngredient dbIngredient in dbProd.ParentIngredients)
                    {
                        if (product.ParentIngredients.All(pi => pi.ParentProductId != dbIngredient.ParentProductId && pi.ChildProductId != dbIngredient.ChildProductId))
                        {
                            _context.Set<ProductIngredient>().Remove(dbIngredient);
                        }
                    }
                    foreach (ProductPricing dbPricing in dbProd.ProductPricings)
                    {
                        if (product.ProductPricings.All(pp => pp.Id != dbPricing.Id))
                        {
                            _context.Set<ProductPricing>().Remove(dbPricing);
                        }
                    }
                    // copy current (incoming) values to db
                    _context.Entry(dbProd).CurrentValues.SetValues(product);
                    var ingredientPairs = from curr in product.ParentIngredients//.Select(pi => pi.IngredientProduct)
                                          join db in dbProd.ParentIngredients//.Select(pi => pi.IngredientProduct)
                                            on new { curr.ParentProductId, curr.ChildProductId } equals
                                            new { db.ParentProductId, db.ChildProductId } into grp
                                          from db in grp.DefaultIfEmpty()
                                          select new { curr, db };
                    foreach (var pair in ingredientPairs)
                    {
                        if (pair.db != null)
                        {
                            _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                            _context.Set<ProductIngredient>().Update(pair.db);
                        }
                        else
                        {
                            await _context.Set<ProductIngredient>().AddAsync(pair.curr);
                        }
                    }
                    var pricingPairs = from curr in product.ProductPricings//.Select(pi => pi.IngredientProduct)
                                       join db in dbProd.ProductPricings//.Select(pi => pi.IngredientProduct)
                                         on curr.Id equals db.Id into grp
                                       from db in grp.DefaultIfEmpty()
                                       select new { curr, db };
                    foreach (var pair in pricingPairs)
                    {
                        if (pair.db != null)
                        {
                            _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                            _context.Set<ProductPricing>().Update(pair.db);
                        }
                        else
                        {
                            await _context.Set<ProductPricing>().AddAsync(pair.curr);
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();
            dto = _mapper.Map<ProductDto>(product);
            return dto;
        }

        public async Task<bool> DeleteAsync(Guid productId)
        {
            Product product = await _context.Products
                .FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Product not found");
        }
    }
}