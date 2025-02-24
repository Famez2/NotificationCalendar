using Microsoft.AspNetCore.Mvc;

namespace NotificationCalendar.Api.Controllers.V1.Note;

[ApiController]
[Route("api/calendar")]
public class NoteController : ControllerBase
{
    [HttpGet]
    public void GetNothing()
    {
        
    }
}
