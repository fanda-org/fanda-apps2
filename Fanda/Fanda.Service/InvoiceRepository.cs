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
    public interface IInvoiceRepository
    {
        Task<List<InvoiceDto>> GetAllAsync(Guid yearId);

        Task<InvoiceDto> GetByIdAsync(Guid invoiceId);

        Task<InvoiceDto> SaveAsync(Guid yearId, InvoiceDto dto);

        Task<bool> DeleteAsync(Guid invoiceId);

        string ErrorMessage { get; }
    }

    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public InvoiceRepository(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<InvoiceDto>> GetAllAsync(Guid yearId)
        {
            if (yearId == null || yearId == Guid.Empty)
            {
                throw new ArgumentNullException("yearId", "Year id is missing");
            }

            List<Invoice> invoices = await _context.Invoices
                .Where(p => p.YearId == yearId)
                .AsNoTracking()
                //.ProjectTo<InvoiceViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return _mapper.Map<List<InvoiceDto>>(invoices);
        }

        public async Task<InvoiceDto> GetByIdAsync(Guid invoiceId)
        {
            InvoiceDto invoice = await _context.Invoices
                .ProjectTo<InvoiceDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(inv => inv.Id == invoiceId);

            if (invoice != null)
            {
                return invoice;
            }

            throw new KeyNotFoundException("Invoice not found");
        }

        public async Task<InvoiceDto> SaveAsync(Guid yearId, InvoiceDto dto)
        {
            if (yearId == null || yearId == Guid.Empty)
            {
                throw new ArgumentNullException("yearId", "Year id is missing");
            }

            Invoice invoice = _mapper.Map<Invoice>(dto);
            if (invoice.Id == Guid.Empty)
            {
                invoice.YearId = yearId;
                invoice.DateCreated = DateTime.UtcNow;
                invoice.DateModified = null;
                await _context.Invoices.AddAsync(invoice);
            }
            else
            {
                Invoice dbInvoice = await _context.Invoices
                    .Where(i => i.Id == invoice.Id)
                    .Include(i => i.InvoiceItems).ThenInclude(ii => ii.Stock)
                    .SingleOrDefaultAsync();
                if (dbInvoice == null)
                {
                    invoice.DateCreated = DateTime.UtcNow;
                    invoice.DateModified = null;
                    await _context.Invoices.AddAsync(invoice);
                }
                else
                {
                    invoice.DateModified = DateTime.UtcNow;
                    // delete all linet items that no longer exists
                    foreach (InvoiceItem dbLineItem in dbInvoice.InvoiceItems)
                    {
                        if (invoice.InvoiceItems.All(ii => ii.InvoiceItemId != dbLineItem.InvoiceItemId))
                        {
                            _context.Set<InvoiceItem>().Remove(dbLineItem);
                        }
                    }
                    // copy current (incoming) values to db
                    _context.Entry(dbInvoice).CurrentValues.SetValues(invoice);
                    var itemPairs = from curr in invoice.InvoiceItems//.Select(pi => pi.IngredientProduct)
                                    join db in dbInvoice.InvoiceItems//.Select(pi => pi.IngredientProduct)
                                      on curr.InvoiceItemId equals db.InvoiceItemId into grp
                                    from db in grp.DefaultIfEmpty()
                                    select new { curr, db };
                    foreach (var pair in itemPairs)
                    {
                        if (pair.db != null)
                        {
                            _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                        }
                        else
                        {
                            await _context.Set<InvoiceItem>().AddAsync(pair.curr);
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();
            dto = _mapper.Map<InvoiceDto>(invoice);
            return dto;
        }

        public async Task<bool> DeleteAsync(Guid invoiceId)
        {
            Invoice invoice = await _context.Invoices
                .FindAsync(invoiceId);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Invoice not found");
        }
    }
}