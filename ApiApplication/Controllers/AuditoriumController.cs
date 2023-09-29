using ApiApplication.Domain.ReadModels;
using ApiApplication.Dto;
using ApiApplication.UseCases.Queries.GetSeatsInAuditorium;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]/")]
[ApiController]
public class AuditoriumsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuditoriumsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<Auditorium>>> GetShowtimes()
    {
        var query = new GetAuditoriumsQuery();
        var auditoriums = await _mediator.Send(query);
        return Ok(auditoriums);
    }
}