using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Infrastructure.Messaging.Requests.ReservationsRequests;
using TM.Infrastructure.Messaging.Requests.UsersRequests;
using TM.Infrastructure.Messaging.Responses.ReservationsResponses;
using TM.Infrastructure.Messaging.Responses.UsersResponses;

namespace TM.ApplicationServices.Interfaces
{
    public interface IReservationsManagementService
    {
        Task<CreateReservationResponse> CreateReservation(CreateReservationRequest request);
        Task<GetReservationResponse> GetReservation(GetReservationRequest request);
        Task<ReservationViewModel> GetReservationById(int id);
        Task<DeleteReservationResponse> DeleteReservation(DeleteReservationRequest request);
        Task<UpdateReservationResponse> UpdateReservation(UpdateReservationRequest request);
        Task<GetReservationResponse> SearchReservationByUserId(int id);
    }
}
