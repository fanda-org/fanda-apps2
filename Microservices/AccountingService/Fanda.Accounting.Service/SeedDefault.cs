using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

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