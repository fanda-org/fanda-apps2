using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace Fanda.Core.SqlClients
{
    public class MsSqlClient : IDbClient
    {
        public MsSqlClient(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }

        public DbConnection Connection { get; }

        public void Dispose() => Connection.Dispose();
    }
}