namespace Fanda.Core.SqlClients
{
    public class MySqlClient : IDbClient
    {
        public System.Data.Common.DbConnection Connection { get; }

        public MySqlClient(string connectionString)
        {
            Connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}
