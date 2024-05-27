using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Data.Entities;

namespace TM.Data.Contexts
{
    public class TravelMeDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public TravelMeDbContext(DbContextOptions<TravelMeDbContext> options) : base(options) { }

        public TravelMeDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TravelMeDB;Integrated Security=True;");
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
}
