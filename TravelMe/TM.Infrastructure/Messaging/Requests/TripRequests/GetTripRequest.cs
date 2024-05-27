using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Infrastructure.Messaging.Requests.TripRequests
{
    public class GetTripRequest : ServiceRequestBase
    {
        public bool IsActive { get; set; }
        public GetTripRequest(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
