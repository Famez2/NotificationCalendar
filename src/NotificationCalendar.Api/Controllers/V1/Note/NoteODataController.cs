using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using NotificationCalendar.Abstractions.Application;
using NotificationCalendar.Persistence;
using System.Text;

namespace NotificationCalendar.Api.Controllers.V1.Note;

public class NoteODataController : ODataController
{
    private readonly INotificationCalendarDbContext _notificationCalendarDbContext;
    private readonly IScvGenerateService _scvGenerateService;

    public NoteODataController(
        INotificationCalendarDbContext notificationCalendarDbContext,
        IScvGenerateService scvGenerateService)
    {
        _notificationCalendarDbContext = notificationCalendarDbContext;
        _scvGenerateService = scvGenerateService;
    }

    [EnableQuery]
    public async Task<IActionResult> GetNotesAsync()
    {
        var notes = await _notificationCalendarDbContext.Note.ToListAsync();

        var csvContent = await _scvGenerateService.GenerateCsvAsync(notes.Select(n => n.Id).ToList());

        var fileName = "NotesExport.csv";
        var byteArray = Encoding.UTF8.GetBytes(csvContent);
        var stream = new MemoryStream(byteArray);

        return File(stream, "text/csv", fileName);
    }
}
