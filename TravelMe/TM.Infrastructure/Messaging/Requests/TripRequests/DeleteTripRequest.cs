using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Infrastructure.Messaging.Requests.TripRequests
{
    public class DeleteTripRequest: IntegerRequestBase
    {
        public DeleteTripRequest(int id) : base(id)
        {

        }
    }
}
