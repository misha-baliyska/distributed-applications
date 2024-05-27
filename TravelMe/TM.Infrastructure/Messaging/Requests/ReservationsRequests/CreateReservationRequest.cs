using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Infrastructure.Messaging.Requests.UsersRequests;

namespace TM.Infrastructure.Messaging.Requests.ReservationsRequests
{
    public class CreateReservationRequest : ServiceRequestBase
    {
        public ReservationModel Reservation { get; set; }
        public CreateReservationRequest(ReservationModel reservation)
        {
            this.Reservation = reservation;
        }
    }
}
