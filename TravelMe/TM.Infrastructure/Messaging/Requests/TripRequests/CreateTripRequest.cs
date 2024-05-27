using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Infrastructure.Messaging.Requests.ReservationsRequests;

namespace TM.Infrastructure.Messaging.Requests.TripRequests
{
    public class CreateTripRequest: ServiceRequestBase
    {
        public TripModel Trip { get; set; }
        public CreateTripRequest(TripModel trip)
        {
            this.Trip = trip;
        }
    }
}
