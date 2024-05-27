using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Repositories.Interfaces;

namespace TM.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        public IUsersRepository Users { get; set; }

        public ITripsRepository Trips { get; set; }

        public IReservationsRepository Reservations { get; set; }

        public DbContext Context { get { return context; } }

        public UnitOfWork(DbContext context)
        {
            this.context = context;
            Users = new UsersRepository(context);
            Trips = new TripsRepository(context);
            Reservations = new ReservationsRepository(context);
        }

        public void Dispose() => this.Dispose(true);

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context?.Dispose();
            }
        }

        public Task<int> SaveChangesAsync()
        {
            return this.context.SaveChangesAsync();
        }
    }
}
