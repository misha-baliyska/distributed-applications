using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Infrastructure.Messaging.Requests.UsersRequests
{
    public class DeleteUserRequest : IntegerRequestBase
    {
        public DeleteUserRequest(int id): base(id)
        {
                
        }
    }
}
