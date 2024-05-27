using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Infrastructure.Messaging.Responses.ReservationsResponses
{
    public class GetReservationResponse : ServiceResponseBase
    {
        public List<ReservationViewModel>? Reservations { get; set; }
    }
}
