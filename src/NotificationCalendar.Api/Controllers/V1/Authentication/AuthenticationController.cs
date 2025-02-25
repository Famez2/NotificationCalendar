using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationCalendar.Api.Contracts;
using NotificationCalendar.Application.Handlers.Authentication.Commands.SignUpUser;
using NotificationCalendar.Contracts.Note;

namespace NotificationCalendar.Api.Controllers.V1.Authentication;

[Route("api/auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("signup")]
    public async Task<ActionResult<ApiResponseV1>> SignupUserAsync(
        [FromBody] SignupFormDTO signupFormDto,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<SingUpUserCommand>(signupFormDto);

        await _mediator.Send(command, cancellationToken);

        return new ApiResponseV1();
    }
}