using ApiApplication.Dto;
using ApiApplication.UseCases.Reservations.ConfirmReservation;
using ApiApplication.UseCases.Reservations.ReserveAListOfSeats;
using ApiApplication.UseCases.Reservations.ReserveARangeOfSeats;
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

    [HttpPost("/seats")]
    public async Task<ActionResult<ReservationDto>> CreateReservation([FromBody] ReserveAListOfSeats command)
    {
        var reservation = await _mediator.Send(command);
        return Ok(ReservationDto.FromEntity(reservation));
    }

    [HttpPost("/range")]
    public async Task<ActionResult<ReservationDto>> CreateReservation([FromBody] ReserveARangeOfSeats command)
    {
        var reservation = await _mediator.Send(command);
        return Ok(ReservationDto.FromEntity(reservation));
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmReservation([FromBody] ConfirmReservation command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}