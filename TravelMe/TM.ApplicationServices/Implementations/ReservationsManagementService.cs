using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.ApplicationServices.Interfaces;
using TM.Data.Entities;
using TM.Data.Entities.Enums;
using TM.Infrastructure.Messaging.Requests.ReservationsRequests;
using TM.Infrastructure.Messaging.Responses.ReservationsResponses;
using TM.Infrastructure.Messaging.Responses.TripResponses;
using TM.Repositories.Interfaces;

namespace TM.ApplicationServices.Implementations
{
    public class ReservationsManagementService : BaseManagementService, IReservationsManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservationsManagementService(ILogger<ReservationsManagementService> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateReservationResponse> CreateReservation(CreateReservationRequest request)
        {
            User user = await _unitOfWork.Users.GetByIdAsync(request.Reservation.UserId);
            Trip trip = await _unitOfWork.Trips.GetByIdAsync(request.Reservation.TripId);

            decimal price = 0;
            if (request.Reservation.LuggageSize == LuggageSize.Medium)
            {
                price = trip.Price + trip.Price * (decimal)0.25;
            }
            else if (request.Reservation.LuggageSize == LuggageSize.Large)
            {
                price = trip.Price + trip.Price * (decimal)0.5;
            }
            else
            {
                price = trip.Price;
            }

            _unitOfWork.Reservations.Insert(new()
            {
                User = user,
                Trip = trip,
                Id = request.Reservation.Id,
                UserId = request.Reservation.UserId,
                TripId = request.Reservation.TripId,
                Price = price,
                LuggageSize = request.Reservation.LuggageSize,
                Note = request.Reservation.Note,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = 1,
                IsActivated = true,
            });

            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<DeleteReservationResponse> DeleteReservation(DeleteReservationRequest request)
        {
            var reservation = await _unitOfWork.Reservations.GetByIdAsync(request.Id);

            if (reservation == null)
            {
                _logger.LogError("Reservation with identifier {trip.Id} not found", request.Id);
                throw new Exception("");
            }

            _unitOfWork.Reservations.Delete(reservation);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<GetReservationResponse> GetReservation(GetReservationRequest request)
        {
            GetReservationResponse response = new() { Reservations = new() };

            var reservations = await _unitOfWork.Reservations.GetAllAsync(request.IsActive);

            foreach (var reservation in reservations)
            {
                response.Reservations.Add(new()
                {
                    Id = reservation.Id,
                    UserId = reservation.User.Id,
                    TripId = reservation.TripId,
                    Price = reservation.Price,
                    LuggageSize = reservation.LuggageSize,
                    Note = reservation.Note
                });
            }

            return response;
        }

        public async Task<ReservationViewModel> GetReservationById(int id)
        {
            var reservation = await _unitOfWork.Reservations.GetByIdAsync(id);
            if (reservation == null)
            {
                throw new Exception("");
            }
            ReservationViewModel result = new ReservationViewModel()
            {
                Id = id,
                Price = reservation.Price,
                UserId = reservation.UserId,
                TripId = reservation.TripId,
                LuggageSize = reservation.LuggageSize,
                Note = reservation.Note
            };
            return result;
        }

        public async Task<UpdateReservationResponse> UpdateReservation(UpdateReservationRequest request)
        {
            var reservation = await _unitOfWork.Reservations.GetByIdAsync(request.ReservationId);
            if (reservation == null)
            {
                _logger.LogError("Reservation with identifier {request.ReservationId} not found", request.ReservationId);
                throw new Exception("");
            }

            reservation.Id = request.ReservationId;
            reservation.UserId = request.Reservation.UserId;
            reservation.TripId = request.Reservation.TripId;
            reservation.Price = request.Reservation.Price;
            reservation.LuggageSize = request.Reservation.LuggageSize;
            reservation.Note = request.Reservation.Note;

            _unitOfWork.Reservations.Update(reservation);

            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<GetReservationResponse> SearchReservationByUserId(int id)
        {
            GetReservationResponse response = new() { Reservations = new() };

            var reservations = await _unitOfWork.Reservations.GetAllAsync();
            var filteredReservations = reservations.Where(x => x.UserId == id).ToList();
            List<ReservationViewModel> result = new List<ReservationViewModel>();
            foreach (var reservation in filteredReservations)
            {
                ReservationViewModel current = new ReservationViewModel()
                {
                    Id = reservation.Id,
                    Price = reservation.Price,
                    UserId = reservation.UserId,
                    TripId = reservation.TripId,
                    LuggageSize = reservation.LuggageSize,
                    Note = reservation.Note
                };

                response.Reservations.Add(current);
            }

            return response;
        }
    
    }
}
