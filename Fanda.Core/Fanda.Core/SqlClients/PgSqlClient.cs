namespace Fanda.Core.SqlClients
{
    public class PgSqlClient : IDbClient
    {
        public System.Data.Common.DbConnection Connection { get; }

        public PgSqlClient(string connectionString)
        {
            Connection = new Npgsql.NpgsqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}
