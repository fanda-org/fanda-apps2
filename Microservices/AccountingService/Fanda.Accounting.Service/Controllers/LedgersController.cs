﻿using Fanda.Accounting.Domain;
using Fanda.Accounting.Repository;
using Fanda.Accounting.Repository.Dto;
using Fanda.Core.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Accounting.Service.Controllers
{
    public class LedgersController :
        SubController<ILedgerRepository, Ledger, LedgerDto, LedgerListDto>
    {
        public LedgersController(ILedgerRepository repository) : base(repository)
        {
        }
    }
}