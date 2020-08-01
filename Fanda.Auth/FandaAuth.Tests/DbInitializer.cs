using FandaAuth.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FandaAuth.Tests
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
