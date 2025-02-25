using AutoMapper;
using NotificationCalendar.Application.Handlers.Note.Commands.AddNote;
using NotificationCalendar.Contracts.Note;

namespace NotificationCalendar.Api.Mappings;

public class NoteMappingProfile : Profile
{
    public NoteMappingProfile()
    {
        CreateMap<AddNotesDTO, AddNotesCommand>();

        CreateMap<AddNotesDTO.NoteInfoModel, AddNotesCommand.NoteInfoModel>();

        CreateMap<AddNotesDTO.HeaderInfoModel, AddNotesCommand.HeaderInfoModel>();
    }
}
