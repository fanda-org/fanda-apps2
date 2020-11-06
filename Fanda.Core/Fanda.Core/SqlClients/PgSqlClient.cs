using Npgsql;
using System.Data.Common;

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