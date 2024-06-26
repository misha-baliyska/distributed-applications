﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Repositories.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        DbContext Context { get; }

        IUsersRepository Users { get; }

        ITripsRepository Trips { get; }

        IReservationsRepository Reservations { get; }

        Task<int> SaveChangesAsync();
    }
}
