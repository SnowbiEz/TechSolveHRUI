using System;
using Humanizer;
using Humanizer.Localisation;

namespace TechSolveHR.Models;

public class Attendance
{
    public DateTimeOffset TimeIn { get; set; }

    public DateTimeOffset TimeOut { get; set; }

    public Guid Id { get; set; }

    public string LengthString => Length.Humanize();

    public string OverTimeString => OverTime < TimeSpan.Zero ? "None" : OverTime.Humanize(minUnit: TimeUnit.Minute);

    public string TimeInString => TimeIn.Humanize();

    public string TimeOutString => TimeOut.Humanize();

    public TimeSpan Length => TimeOut - TimeIn;

    public TimeSpan OverTime
    {
        get
        {
            var timeSpan = Length.Subtract(TimeSpan.FromHours(8));
            return timeSpan > TimeSpan.Zero ? timeSpan : TimeSpan.Zero;
        }
    }
}