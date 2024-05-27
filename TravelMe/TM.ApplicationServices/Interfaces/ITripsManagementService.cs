using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Infrastructure.Messaging.Requests.TripRequests;
using TM.Infrastructure.Messaging.Responses.TripResponses;

namespace TM.ApplicationServices.Interfaces
{
    public interface ITripsManagementService
    {
        Task<CreateTripResponse> CreateTrip(CreateTripRequest request);
        Task<GetTripResponse> GetTrip(GetTripRequest request);
        Task<TripViewModel> GetTripById(int id);
        Task<DeleteTripResponse> DeleteTrip(DeleteTripRequest request);
        Task<UpdateTripResponse> UpdateTrip(UpdateTripRequest request);
        Task<GetTripResponse> SearchTripByDestination(string destination);
    }
}
    