using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Data.Entities;
using TM.Repositories.Interfaces;

namespace TM.Repositories.Implementations
{
    public class TripsRepository : Repository<Trip>, ITripsRepository
    {
        public TripsRepository(DbContext context) : base(context) { }

        public override async Task<IEnumerable<Trip>> GetAllAsync(bool isActive = true)
        {
            return await SoftDeleteQueryFilter(this.DbSet, isActive).ToListAsync();
        }
    }
}
