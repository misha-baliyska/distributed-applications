using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Infrastructure.Messaging.Responses.ReservationsResponses;

namespace TM.Infrastructure.Messaging.Responses.TripResponses
{
    public class GetTripResponse : ServiceResponseBase
    {
        public List<TripViewModel>? Trips { get; set; }
    }
}
