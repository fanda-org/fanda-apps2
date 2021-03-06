﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Accounting.Domain;
using Fanda.Accounting.Domain.Context;
using Fanda.Accounting.Repository.Dto;
using Fanda.Core;
using Fanda.Core.Base;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fanda.Accounting.Repository
{
    public interface IJournalRepository :
        ISubRepository<Journal, JournalDto, JournalListDto>
    {
    }

    public class JournalRepository : IJournalRepository
    {
        private readonly AcctContext _context;
        private readonly IMapper _mapper;

        public JournalRepository(AcctContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JournalDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "Id is required");
            }

            var journal = await _context.Journals
                .AsNoTracking()
                .Where(t => t.Id == id)
                .ProjectTo<JournalDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (journal == null)
            {
                throw new NotFoundException("Journal not found");
            }

            return journal;
        }

        public async Task<IEnumerable<JournalDto>> FindAsync(Guid yearId, Expression<Func<Journal, bool>> predicate)
        {
            var newPredicate = PredicateBuilder.New(predicate);
            newPredicate = newPredicate.And(GetSuperIdPredicate(yearId));

            var models = await _context.Journals
                .AsNoTracking()
                .Where(newPredicate)
                .ProjectTo<JournalDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return models;
        }

        public async Task<IEnumerable<JournalDto>> FindAsync(Guid yearId, string expression, params object[] args)
        {
            int newSize = args.Length + 1;
            string newExpression = expression + $" AND YearId==@${newSize - 1}";
            Array.Resize(ref args, newSize);
            args[newSize - 1] = yearId;

            var models = await _context.Journals
                .AsNoTracking()
                .Where(newExpression, args)
                .ProjectTo<JournalDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return models;
        }

        public async Task<JournalDto> CreateAsync(Guid superId, JournalDto model)
        {
            var validationResult = await ValidateAsync(superId, model);
            if (!validationResult.IsValid())
            {
                throw new BadRequestException(validationResult);
            }

            var journal = _mapper.Map<Journal>(model);

            journal.YearId = superId;
            journal.DateCreated = DateTime.UtcNow;
            // Construct Transactions
            journal.Transactions = new List<Transaction>();
            foreach (var ji in journal.JournalItems)
            {
                journal.Transactions.Add(new Transaction
                {
                    Journal = journal,
                    Date = journal.Date,
                    DebitLedgerId = journal.JournalSign == "D" ? journal.LedgerId : ji.LedgerId,
                    CreditLedgerId = journal.JournalSign == "C" ? journal.LedgerId : ji.LedgerId,
                    Quantity = ji.Quantity,
                    Amount = ji.Amount,
                    Description = ji.Description,
                    Number = journal.Number,
                    ReferenceNumber = ji.ReferenceNumber,
                    ReferenceDate = ji.ReferenceDate,
                    YearId = journal.YearId,
                    DateCreated = DateTime.UtcNow
                });
            }

            await _context.Journals.AddAsync(journal);
            await _context.SaveChangesAsync();
            return _mapper.Map<JournalDto>(journal);
        }

        public async Task UpdateAsync(Guid id, JournalDto model)
        {
            if (id != model.Id)
            {
                throw new BadRequestException("Journal id mismatch");
            }

            var dbJnl = await _context.Journals
                .Where(o => o.Id == model.Id)
                .Include(o => o.JournalItems)
                .Include(o => o.Transactions)
                .FirstOrDefaultAsync();
            if (dbJnl == null)
            {
                throw new NotFoundException("Journal not found");
            }

            var yearId = dbJnl.YearId;
            var validationResult = await ValidateAsync(yearId, model);
            if (!validationResult.IsValid())
            {
                throw new BadRequestException(validationResult);
            }

            // copy current (incoming) values to db
            var jnl = _mapper.Map<Journal>(model);
            jnl.DateCreated = dbJnl.DateCreated;
            jnl.DateModified = DateTime.UtcNow;
            _context.Entry(dbJnl).CurrentValues.SetValues(jnl);

            // delete all journalItems that are no longer exists
            foreach (var dbJnlItem in dbJnl.JournalItems)
            {
                if (jnl.JournalItems.All(ji => ji.JournalItemId != dbJnlItem.JournalItemId))
                {
                    _context.Set<JournalItem>().Remove(dbJnlItem);
                }
            }

            // delete all transactions that are no longer exists
            foreach (var dbTran in dbJnl.Transactions)
            {
                if (jnl.Transactions.All(t => t.Id != dbTran.Id))
                {
                    _context.Transactions.Remove(dbTran);
                }
            }

            #region JournalItems

            var jnlItemPairs = from curr in jnl.JournalItems //OrgContacts.Select(oc => oc.Contact)
                               join db in dbJnl.JournalItems //OrgContacts.Select(oc => oc.Contact)
                                   on curr.JournalItemId equals db.JournalItemId into grp
                               from db in grp.DefaultIfEmpty()
                               select new { curr, db };
            foreach (var pair in jnlItemPairs)
            {
                if (pair.db == null)
                {
                    var jnlItem = new JournalItem
                    {
                        JournalId = jnl.Id,
                        Journal = jnl,
                        LedgerId = pair.curr.LedgerId,
                        Quantity = pair.curr.Quantity,
                        Amount = pair.curr.Amount,
                        Description = pair.curr.Description,
                        ReferenceNumber = pair.curr.ReferenceNumber,
                        ReferenceDate = pair.curr.ReferenceDate
                    };
                    dbJnl.JournalItems.Add(jnlItem);
                }
                else
                {
                    _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                }
            }

            #endregion JournalItems

            #region Transactions

            var tranPairs = from curr in jnl.Transactions //OrgAddresses.Select(oa => oa.Address)
                            join db in dbJnl.Transactions //OrgAddresses.Select(oa => oa.Address)
                                on curr.Id equals db.Id into grp
                            from db in grp.DefaultIfEmpty()
                            select new { curr, db };
            foreach (var pair in tranPairs)
            {
                if (pair.db == null)
                {
                    var tran = new Transaction
                    {
                        Number = jnl.Number,
                        Date = jnl.Date,
                        ReferenceNumber = jnl.ReferenceNumber,
                        ReferenceDate = jnl.ReferenceDate,
                        YearId = yearId,
                        DateCreated = DateTime.UtcNow,
                        DateModified = null,
                        DebitLedgerId = pair.curr.DebitLedgerId,
                        CreditLedgerId = pair.curr.CreditLedgerId,
                        Quantity = pair.curr.Quantity,
                        Amount = pair.curr.Amount,
                        Description = pair.curr.Description,
                        JournalId = jnl.Id,
                        Journal = jnl
                    };
                    dbJnl.Transactions.Add(tran);
                }
                else
                {
                    _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                    //_context.Addresses.Update(pair.db);
                }
            }

            #endregion Transactions

            //_context.Organizations.Update(dbOrg);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "Id is required");
            }

            var journal = await _context.Journals.FindAsync(id);
            if (journal == null)
            {
                throw new NotFoundException("Journal not found");
            }

            _context.Journals.Remove(journal);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateAsync(Guid id, bool active)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "Id is required");
            }

            var entity = await _context.Journals.FindAsync(id);
            if (entity == null)
            {
                throw new NotFoundException("Journal not found");
            }

            entity.DateModified = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<JournalListDto> GetAll(Guid? superId)
        {
            return _context.Journals
                .AsNoTracking()
                .Where(j => j.YearId == superId)
                .ProjectTo<JournalListDto>(_mapper.ConfigurationProvider);
        }

        public async Task<bool> AnyAsync(Guid yearId, Expression<Func<Journal, bool>> predicate)
        {
            var newPredicate = PredicateBuilder.New(predicate);
            newPredicate = newPredicate.And(GetSuperIdPredicate(yearId));

            return await _context.Journals.AnyAsync(newPredicate);
        }

        public bool Any(Guid yearId, string expression, params object[] args)
        {
            int newSize = args.Length + 1;
            string newExpression = expression + $" AND YearId==@{newSize - 1}";
            Array.Resize(ref args, newSize);
            args[newSize - 1] = yearId;

            return _context.Journals.Any(newExpression, args);
        }

        public async Task<ValidationErrors> ValidateAsync(Guid superId, JournalDto model)
        {
            model.Errors.Clear();

            #region Check duplicate

            if (await AnyAsync(superId, GetJournalNumberPredicate(model.Number, model.Id)))
            {
                model.Errors.AddError(nameof(model.Number), $"Journal Number '{model.Number}' already exists");
            }

            #endregion Check duplicate

            return model.Errors;
        }

        public virtual Expression<Func<Journal, bool>> GetSuperIdPredicate(Guid? yearId)
        {
            return j => j.YearId == yearId;
        }

        private static ExpressionStarter<Journal> GetJournalNumberPredicate(string journalNumber,
            Guid id = default)
        {
            var numExpression = PredicateBuilder.New<Journal>(j => j.Number == journalNumber);
            //numExpression = numExpression.And(j => j.YearId == superId);
            if (id != Guid.Empty)
            {
                numExpression = numExpression.And(e => e.Id != id);
            }

            return numExpression;
        }

        //public bool Any(string expression, params object[] args)
        //{
        //    return _context.Journals.Any(expression, args);
        //}
    }
}
