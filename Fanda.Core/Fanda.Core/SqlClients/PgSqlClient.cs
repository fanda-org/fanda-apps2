using System.Data.Common;
using Npgsql;

namespace Fanda.Core.SqlClients
{
    public class PgSqlClient : IDbClient
    {
        public PgSqlClient(string connectionString)
        {
            Connection = new NpgsqlConnection(connectionString);
        }

        public DbConnection Connection { get; }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}