using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Infrastructure.Messaging.Requests.UsersRequests
{
    public class GetUserRequest : ServiceRequestBase
    {
        public bool IsActive { get; set; }
        public GetUserRequest(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
