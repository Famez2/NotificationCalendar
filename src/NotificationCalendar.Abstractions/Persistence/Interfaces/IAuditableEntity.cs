﻿namespace NotificationCalendar.Abstractions.Persistence.Interfaces;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }

    DateTime UpdatedAt { get; set; }
}
