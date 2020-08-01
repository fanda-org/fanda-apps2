namespace Fanda.Core.SqlClients
{
    public class SqliteClient : IDbClient
    {
        public System.Data.Common.DbConnection Connection { get; }

        public SqliteClient(string connectionString)
        {
            //Connection = new System.Data.SQLite.SQLiteConnection(connectionString);
            Connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}
