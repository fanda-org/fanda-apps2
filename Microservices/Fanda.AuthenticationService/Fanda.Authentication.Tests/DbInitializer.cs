using Fanda.Authentication.Domain;

namespace Fanda.Authentication.Tests
{
    public class DbInitializer
    {
        public DbInitializer()
        {
        }

        public void Seed(AuthContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    }
}
