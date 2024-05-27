using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Infrastructure.Messaging.Requests.TripRequests;
using TM.Infrastructure.Messaging.Requests.UsersRequests;
using TM.Infrastructure.Messaging.Responses.TripResponses;
using TM.Infrastructure.Messaging.Responses.UsersResponses;

namespace TM.ApplicationServices.Interfaces
{
    public interface IUsersManagementService
    {
        Task<CreateUserResponse> CreateUser(CreateUserRequest request);
        Task<GetUserResponse> GetUser(GetUserRequest request);
        Task<UserViewModel> GetUserById(int id);
        Task<GetUserResponse> SearchUserByUsername(string name);
        Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request);
        Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request);
    }
}
