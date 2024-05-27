using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Infrastructure.Messaging.Requests.ReservationsRequests;

namespace TM.Infrastructure.Messaging.Requests.TripRequests
{
    public class UpdateTripRequest : ServiceRequestBase
    {
        public int TripId { get; set; }
        public TripModel Trip { get; set; }
        public UpdateTripRequest(int tripId, TripModel trip)
        {
            this.TripId = tripId;
            this.Trip = trip;
        }
    }
}
