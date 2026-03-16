namespace CleanTeeth.Domain.ValueObjects;
public sealed class TimeInterval
{
    public DateTime Start { get; }
    public DateTime End { get; }
    public TimeInterval(DateTime start, DateTime end)
    {
        if (start > end)
        {
            throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.");
        }

        Start = start;
        End = end;
    }
}
