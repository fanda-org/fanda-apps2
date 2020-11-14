using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace Fanda.Core.SqlClients
{
    public class SqliteClient : IDbClient
    {
        public SqliteClient(string connectionString)
        {
            //Connection = new System.Data.SQLite.SQLiteConnection(connectionString);
            Connection = new SqliteConnection(connectionString);
        }

        public DbConnection Connection { get; }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}