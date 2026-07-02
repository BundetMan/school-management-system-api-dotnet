namespace SchoolAPI.DTOs.Schedule;

public record ScheduleSlotDtoForManualCreation(
    TimeSpan StartTime,
    TimeSpan EndTime,
    bool IsOccupied,
    ScheduleResponseDto? Schedule // null if free, populated if this slot is already booked
);