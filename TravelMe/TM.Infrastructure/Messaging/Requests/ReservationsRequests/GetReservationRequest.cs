using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Infrastructure.Messaging.Requests.ReservationsRequests
{
    public class GetReservationRequest : ServiceRequestBase
    {
        public bool IsActive { get; set; }
        public GetReservationRequest(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
