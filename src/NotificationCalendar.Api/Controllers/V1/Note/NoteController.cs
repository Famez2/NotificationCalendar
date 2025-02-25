using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationCalendar.Api.Contracts;
using NotificationCalendar.Application.Handlers.Note.Commands.AddNote;
using NotificationCalendar.Contracts.Note;

namespace NotificationCalendar.Api.Controllers.V1.Note;

[ApiController]
[Route("api/calendar")]
public class NoteController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public NoteController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ApiResponseV1> AddNotesAsync(
        [FromBody] AddNotesDTO request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<AddNotesCommand>(request);

        await _mediator.Send(command, cancellationToken);

        return new ApiResponseV1();
    }
}
