using ApiApplication.Dto;
using ApiApplication.UseCases.Showtimes.CreateShowtime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
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
}