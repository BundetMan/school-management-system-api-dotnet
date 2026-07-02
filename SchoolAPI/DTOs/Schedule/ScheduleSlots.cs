using SchoolAPI.Models.Schedules;

namespace SchoolAPI.DTOs.Schedule;

public static class ScheduleSlots
{
    // 7 fixed slots per weekday
    public static readonly (TimeOnly Start, TimeOnly End)[] WeekdaySlots =
    {
        (new(7,0),  new(8,0)),
        (new(8,0),  new(9,0)),
        (new(9,0),  new(10,0)),
        (new(10,0), new(11,0)),
        (new(14,0), new(15,0)),
        (new(15,0), new(16,0)),
        (new(16,0), new(17,0)),
    };

    // Saturday only has the morning 4 slots
    public static readonly (TimeOnly Start, TimeOnly End)[] SaturdaySlots =
        WeekdaySlots.Take(4).ToArray();

    public static readonly SchoolDay[] Days =
    {
        SchoolDay.Monday,
        SchoolDay.Tuesday,
        SchoolDay.Wednesday,
        SchoolDay.Thursday,
        SchoolDay.Friday,
        SchoolDay.Saturday
    };

    public static (TimeOnly Start, TimeOnly End)[] SlotsFor(SchoolDay day) =>
        day == SchoolDay.Saturday ? SaturdaySlots : WeekdaySlots;
}
