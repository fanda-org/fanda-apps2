using MySql.Data.MySqlClient;
using System;
using System.Data.Common;

namespace Fanda.Core.SqlClients
{
    public class MySqlClient : IDbClient
    {
        public MySqlClient(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public DbConnection Connection { get; }

        public void Dispose()
        {
            Connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}