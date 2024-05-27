using Microsoft.AspNetCore.Mvc;
using TM.ApplicationServices.Interfaces;
using TM.Infrastructure.Messaging.Requests.TripRequests;
using TM.Infrastructure.Messaging.Responses.TripResponses;
using TM.Infrastructure.Messaging.Responses.UsersResponses;
using TM.Infrastructure.Messaging;
using TM.Infrastructure.Messaging.Responses.ReservationsResponses;
using TM.Infrastructure.Messaging.Requests.ReservationsRequests;

namespace TM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class ReservationsController : Controller
    {
        private readonly IReservationsManagementService _reservationService;
        public ReservationsController(IReservationsManagementService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetReservationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] bool isActive = true) => Ok(await _reservationService.GetReservation(new(isActive)));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetReservationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReservationById([FromRoute] int id) => Ok(await _reservationService.GetReservationById(id));

        [HttpPost]
        [ProducesResponseType(typeof(CreateReservationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationModel reservation) => Ok(await _reservationService.CreateReservation(new(reservation)));

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteReservationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteReservation([FromRoute] int id) => Ok(await _reservationService.DeleteReservation(new(id)));

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateReservationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReservation([FromRoute] int id, [FromBody] UpdateReservationRequest updateRequest) => Ok(await _reservationService.UpdateReservation(new(id, updateRequest.Reservation)));


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetReservationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchReservationByUserId(int id) => Ok(await _reservationService.SearchReservationByUserId(id));
    }
}
