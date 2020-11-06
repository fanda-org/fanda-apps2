using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanda.Accounting.Repository;
using Fanda.Accounting.Repository.Dto;
using Fanda.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fanda.Accounting.Service
{
    public class SeedDefault
    {
        private readonly ILogger<SeedDefault> _logger;
        private readonly IServiceProvider _provider;
        //private readonly AppSettings _settings;

        public SeedDefault(IServiceProvider provider /*, IOptions<AppSettings> options*/)
        {
            _provider = provider;
            //_settings = options.Value;
            _logger = _provider.GetRequiredService<ILogger<SeedDefault>>();
        }
    }
}