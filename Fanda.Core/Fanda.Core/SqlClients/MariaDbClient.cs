namespace Fanda.Core.SqlClients
{
    public class MariaDbClient : IDbClient
    {
        public System.Data.Common.DbConnection Connection { get; }

        public MariaDbClient(string connectionString)
        {
            Connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}
