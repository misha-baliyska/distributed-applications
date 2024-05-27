using TM.Data.Entities;
using TM.Infrastructure.Messaging.Responses.UsersResponses;

namespace TM.Website
{
    public static class AuthUser
    {
        public static UserViewModel LoggedUser { get; set; }
    }
}
