using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Fanda.Core.SqlClients
{
    public class MariaDbClient : IDbClient
    {
        public MariaDbClient(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public DbConnection Connection { get; }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}