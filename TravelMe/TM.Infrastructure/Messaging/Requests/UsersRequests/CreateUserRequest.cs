using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Infrastructure.Messaging.Requests.UsersRequests
{
    public class CreateUserRequest: ServiceRequestBase
    {
        public UserModel User { get; set; }
        public CreateUserRequest(UserModel user)
        {
            this.User = user;    
        }
    }
}
