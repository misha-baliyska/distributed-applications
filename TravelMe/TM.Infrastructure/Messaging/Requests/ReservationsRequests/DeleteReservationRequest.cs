using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Infrastructure.Messaging.Requests.ReservationsRequests
{
    public class DeleteReservationRequest : IntegerRequestBase
    {
        public DeleteReservationRequest(int id) : base(id)
        {

        }
    }
}
