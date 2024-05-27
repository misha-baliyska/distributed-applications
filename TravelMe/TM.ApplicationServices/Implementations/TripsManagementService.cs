using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.ApplicationServices.Interfaces;
using TM.Data.Entities;
using TM.Infrastructure.Messaging.Requests.TripRequests;
using TM.Infrastructure.Messaging.Responses.TripResponses;
using TM.Infrastructure.Messaging.Responses.UsersResponses;
using TM.Repositories.Interfaces;

namespace TM.ApplicationServices.Implementations
{
    public class TripsManagementService : BaseManagementService, ITripsManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TripsManagementService(ILogger<TripsManagementService> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateTripResponse> CreateTrip(CreateTripRequest request)
        {
            _unitOfWork.Trips.Insert(new()
            {
                Id = request.Trip.Id,
                Destination = request.Trip.Destination,
                DateOfDeparture = request.Trip.DateOfDeparture,
                DateOfReturn = request.Trip.DateOfReturn,
                Seats = request.Trip.Seats,
                Price = request.Trip.Price,
                Description = request.Trip.Description,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = 1,
                IsActivated = true
            });

            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<DeleteTripResponse> DeleteTrip(DeleteTripRequest request)
        {
            var trip = await _unitOfWork.Trips.GetByIdAsync(request.Id);

            if (trip == null)
            {
                _logger.LogError("Trip with identifier {trip.Id} not found", request.Id);
                throw new Exception("");
            }

            _unitOfWork.Trips.Delete(trip);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<GetTripResponse> GetTrip(GetTripRequest request)
        {
            GetTripResponse response = new() { Trips = new() };

            var trips = await _unitOfWork.Trips.GetAllAsync(request.IsActive);

            foreach (var trip in trips)
            {
                response.Trips.Add(new()
                {
                    Id = trip.Id,
                    Destination = trip.Destination,
                    DateOfDeparture = trip.DateOfDeparture,
                    DateOfReturn = trip.DateOfReturn,
                    Seats = trip.Seats,
                    Description = trip.Description,
                    Price = trip.Price,
                });
            }

            return response;
        }

        public async Task<TripViewModel> GetTripById(int id)
        {
            var user = await _unitOfWork.Trips.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("");
            }
            TripViewModel result = new TripViewModel()
            {
                Id = id,
                Destination = user.Destination,
                Description = user.Description,
                DateOfDeparture= user.DateOfDeparture,
                DateOfReturn= user.DateOfReturn,
                Seats= user.Seats,
                Price = user.Price,
            };
            return result;
        }

        public async Task<UpdateTripResponse> UpdateTrip(UpdateTripRequest request)
        {
            var trip = await _unitOfWork.Trips.GetByIdAsync(request.TripId);
            if (trip == null)
            {
                _logger.LogError("Trip with identifier {request.TripId} not found", request.TripId);
                throw new Exception("");
            }

            trip.Id = request.Trip.Id;
            trip.Destination = request.Trip.Destination;
            trip.DateOfDeparture = request.Trip.DateOfDeparture;
            trip.DateOfReturn = request.Trip.DateOfReturn;
            trip.Seats = request.Trip.Seats;
            trip.Description = request.Trip.Description;
            trip.Price = request.Trip.Price;

            _unitOfWork.Trips.Update(trip);
            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<GetTripResponse> SearchTripByDestination(string destination)
        {
            GetTripResponse response = new() { Trips = new() };

            var trips = await _unitOfWork.Trips.GetAllAsync();
            var filteredTrips = trips.Where(x => x.Destination == destination).ToList();
            List<TripViewModel> result = new List<TripViewModel>();
            foreach (var trip in filteredTrips)
            {
                TripViewModel current = new TripViewModel()
                {
                    Id = trip.Id,
                    Destination = trip.Destination,
                    Description = trip.Description,
                    DateOfDeparture = trip.DateOfDeparture,
                    DateOfReturn = trip.DateOfReturn,
                    Seats = trip.Seats,
                    Price = trip.Price,
                };

                response.Trips.Add(current);
            }

            return response;
        }
    }
}
