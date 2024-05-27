using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Infrastructure.Messaging.Requests.UsersRequests
{
    public class UpdateUserRequest : ServiceRequestBase
    {
        public int UserId { get; set; }
        public UserModel User { get; set; }

        public UpdateUserRequest(int userId, UserModel user)
        {
            this.UserId = userId;
            this.User = user;
        }
    }
}
