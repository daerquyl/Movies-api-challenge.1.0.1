using ApiApplication.Dto;
using ApiApplication.UseCases.Commands.Reservations.ConfirmReservation;
using ApiApplication.UseCases.Commands.Reservations.ReserveAListOfSeats;
using ApiApplication.UseCases.Commands.Reservations.ReserveARangeOfSeats;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ReservationController: ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/reserve/seats")]
    public async Task<ActionResult<ReservationDto>> CreateReservation([FromBody] ReserveAListOfSeatsCommand command)
    {
        var reservation = await _mediator.Send(command);
        return Ok(ReservationDto.FromEntity(reservation));
    }

    [HttpPost("/reserve/range")]
    public async Task<ActionResult<ReservationDto>> CreateReservation([FromBody] ReserveARangeOfSeatsCommand command)
    {
        var reservation = await _mediator.Send(command);
        return Ok(ReservationDto.FromEntity(reservation));
    }

    [HttpPost("/confirm")]
    public async Task<IActionResult> ConfirmReservation([FromBody] ConfirmReservationCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}