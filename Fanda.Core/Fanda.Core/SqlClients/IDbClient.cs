using System;

namespace Fanda.Core.SqlClients
{
    public interface IDbClient : IDisposable
    {
        public System.Data.Common.DbConnection Connection { get; }
    }
}
