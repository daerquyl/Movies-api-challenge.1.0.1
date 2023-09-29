using ApiApplication.Domain.ReadModels;
using ApiApplication.Dto;
using ApiApplication.UseCases.Commands.Showtimes.CreateShowtime;
using ApiApplication.UseCases.Queries.GetSeatsInAuditorium;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]/")]
[ApiController]
public class ShowtimesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShowtimesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ShowtimeDto>> CreateShowtime([FromBody] CreateShowtimeCommand command)
    {
        var showtime = await _mediator.Send(command);
        return Ok(ShowtimeDto.FromEntity(showtime));
    }

    [HttpGet]
    public async Task<ActionResult<List<ShowtimeWithReservations>>> GetShowtimes()
    {
        var query = new GetShowtimesQuery();
        var showtimes = await _mediator.Send(query);
        return Ok(showtimes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShowtimeWithReservations>> GetShowtimeWithReservations(int id)
    {
        var query = new GetShowtimeWithReservationsQuery(id);
        var showtime = await _mediator.Send(query);
        return Ok(showtime);
    }

    [HttpGet("{id}/seats")]
    public async Task<ActionResult<ShowtimeWithReservations>> GetShowtimeWithSeats(int id)
    {
        var query = new GetShowtimeWithSeatsQuery(id);
        var showtime = await _mediator.Send(query);
        return Ok(showtime);
    }
}