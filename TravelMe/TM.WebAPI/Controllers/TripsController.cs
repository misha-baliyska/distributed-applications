using Microsoft.AspNetCore.Mvc;
using TM.ApplicationServices.Interfaces;
using TM.Infrastructure.Messaging.Requests.UsersRequests;
using TM.Infrastructure.Messaging.Responses.UsersResponses;
using TM.Infrastructure.Messaging;
using TM.Infrastructure.Messaging.Responses.TripResponses;
using TM.Infrastructure.Messaging.Requests.TripRequests;

namespace TM.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class TripsController : Controller
    {
        private readonly ITripsManagementService _tripService;
        public TripsController(ITripsManagementService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetTripResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] bool isActive = true) => Ok(await _tripService.GetTrip(new(isActive)));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTripById([FromRoute] int id) => Ok(await _tripService.GetTripById(id));

        [HttpPost]
        [ProducesResponseType(typeof(CreateTripResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTrip([FromBody] TripModel user) => Ok(await _tripService.CreateTrip(new(user)));

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteTripResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTrip([FromRoute] int id) => Ok(await _tripService.DeleteTrip(new(id)));

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateTripResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTrip([FromRoute] int id, [FromBody] UpdateTripRequest updateRequest) => Ok(await _tripService.UpdateTrip(new(id, updateRequest.Trip)));

        [HttpGet("{destination}")]
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchTripByDestination(string destination) => Ok(await _tripService.SearchTripByDestination(destination));
    }
}
