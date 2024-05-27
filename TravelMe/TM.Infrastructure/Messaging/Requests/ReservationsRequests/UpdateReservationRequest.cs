using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Infrastructure.Messaging.Requests.ReservationsRequests
{
    public class UpdateReservationRequest : ServiceRequestBase
    {
        public int ReservationId { get; set; }
        public ReservationModel Reservation { get; set; }
        public UpdateReservationRequest(int reservationId, ReservationModel reservation)
        {
            this.ReservationId = reservationId;
            this.Reservation = reservation;
        }
    }
}
