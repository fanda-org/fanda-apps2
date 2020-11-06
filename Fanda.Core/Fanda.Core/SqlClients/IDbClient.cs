using System;
using System.Data.Common;

namespace Fanda.Core.SqlClients
{
    public interface IDbClient : IDisposable
    {
        public DbConnection Connection { get; }
    }
}