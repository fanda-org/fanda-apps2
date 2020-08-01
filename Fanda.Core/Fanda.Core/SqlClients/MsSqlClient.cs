namespace Fanda.Core.SqlClients
{
    public class MsSqlClient : IDbClient
    {
        public System.Data.Common.DbConnection Connection { get; }

        public MsSqlClient(string connectionString)
        {
            Connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();
    }
}
