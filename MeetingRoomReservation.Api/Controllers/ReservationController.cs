using MediatR;
using MeetingRoomReservation.Api.Commands;
using MeetingRoomReservation.Api.Queries;
using MeetingRoomReservation.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {
        private readonly IMediator _mediator;

        public ReservationController(Mediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetReservations")]
        public async Task<IActionResult> GetReservations(CancellationToken cancellationToken)
        {
            var reservations = await _mediator.Send(new GetReservationsQuery(), cancellationToken);

            if (reservations == null || !reservations.Any())
                return NotFound();

            return Ok(reservations);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetReservationById(Guid id, CancellationToken cancellationToken)
        {
            var reservation = await _mediator.Send(new GetReservationByIdQuery(id), cancellationToken);

            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(CreateReservationDto createReservationDto, CancellationToken cancellationToken)
        {
            var reservation = await _mediator.Send(new CreateReservationCommand(createReservationDto), cancellationToken);

            return CreatedAtAction(nameof(GetReservationById), new { id = reservation }, reservation);
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateReservation(Guid id, UpdateReservationDto updateReservationDto, CancellationToken cancellationToken)
        {
            var reservation = await _mediator.Send(new UpdateReservationCommand(id, updateReservationDto), cancellationToken);

            if (reservation == null)
                return NotFound();  

            return Ok(reservation);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteReservationAsync(Guid id)
        {
            var result = await _mediator.Send(new DeleteReservationCommand(id));

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
