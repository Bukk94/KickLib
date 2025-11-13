namespace KickLib.Models.v1.Moderation;

/// <summary>
///     Represents a timeout duration for user moderation with built-in validation.
/// </summary>
public readonly struct TimeoutDuration : IEquatable<TimeoutDuration>, IComparable<TimeoutDuration>
{
    /// <summary>
    ///     Minimum allowed timeout duration (1 minute).
    /// </summary>
    public static readonly TimeoutDuration Min = new(1);

    /// <summary>
    ///     Maximum allowed timeout duration (10080 minutes - 7 days).
    /// </summary>
    public static readonly TimeoutDuration Max = new(10080);

    /// <summary>
    ///     Duration in minutes.
    /// </summary>
    public int Minutes { get; }

    /// <summary>
    ///     Duration as TimeSpan.
    /// </summary>
    public TimeSpan TimeSpan => TimeSpan.FromMinutes(Minutes);

    /// <summary>
    ///     Creates a new timeout duration from minutes.
    /// </summary>
    /// <param name="minutes">Duration in minutes (1-10080).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when minutes is outside the valid range.</exception>
    public TimeoutDuration(int minutes)
    {
        if (minutes < 1 || minutes > 10080)
        {
            throw new ArgumentOutOfRangeException(nameof(minutes), 
                $"Duration must be between {Min.Minutes} and {Max.Minutes} minutes.");
        }

        Minutes = minutes;
    }

    /// <summary>
    ///     Creates a timeout duration from a TimeSpan.
    /// </summary>
    /// <param name="timeSpan">The TimeSpan to convert.</param>
    /// <returns>A TimeoutDuration instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the TimeSpan is outside the valid range.</exception>
    public static TimeoutDuration FromTimeSpan(TimeSpan timeSpan)
    {
        var minutes = (int)timeSpan.TotalMinutes;
        return new TimeoutDuration(minutes);
    }

    /// <summary>
    ///     Creates a timeout duration from hours.
    /// </summary>
    /// <param name="hours">Duration in hours.</param>
    /// <returns>A TimeoutDuration instance.</returns>
    public static TimeoutDuration FromHours(double hours)
    {
        return FromTimeSpan(TimeSpan.FromHours(hours));
    }

    /// <summary>
    ///     Creates a timeout duration from days.
    /// </summary>
    /// <param name="days">Duration in days.</param>
    /// <returns>A TimeoutDuration instance.</returns>
    public static TimeoutDuration FromDays(double days)
    {
        return FromTimeSpan(TimeSpan.FromDays(days));
    }

    /// <summary>
    ///     1 minute timeout.
    /// </summary>
    public static TimeoutDuration OneMinute => new(1);

    /// <summary>
    ///     5 minutes timeout.
    /// </summary>
    public static TimeoutDuration FiveMinutes => new(5);

    /// <summary>
    ///     30 minutes timeout.
    /// </summary>
    public static TimeoutDuration ThirtyMinutes => new(30);

    /// <summary>
    ///     1 hour timeout.
    /// </summary>
    public static TimeoutDuration OneHour => new(60);

    /// <summary>
    ///     24 hours timeout.
    /// </summary>
    public static TimeoutDuration OneDay => new(1440);

    /// <summary>
    ///     Implicitly converts TimeoutDuration to int (minutes).
    /// </summary>
    public static implicit operator int(TimeoutDuration duration) => duration.Minutes;

    /// <summary>
    ///     Implicitly converts TimeoutDuration to TimeSpan.
    /// </summary>
    public static implicit operator TimeSpan(TimeoutDuration duration) => duration.TimeSpan;

    /// <summary>
    ///     Explicitly converts int (minutes) to TimeoutDuration.
    /// </summary>
    public static explicit operator TimeoutDuration(int minutes) => new(minutes);

    /// <summary>
    ///     Explicitly converts TimeSpan to TimeoutDuration.
    /// </summary>
    public static explicit operator TimeoutDuration(TimeSpan timeSpan) => FromTimeSpan(timeSpan);

    /// <inheritdoc />
    public bool Equals(TimeoutDuration other) => Minutes == other.Minutes;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is TimeoutDuration other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => Minutes.GetHashCode();

    /// <inheritdoc />
    public int CompareTo(TimeoutDuration other) => Minutes.CompareTo(other.Minutes);

    /// <summary>
    ///     Equality operator.
    /// </summary>
    public static bool operator ==(TimeoutDuration left, TimeoutDuration right) => left.Equals(right);

    /// <summary>
    ///     Inequality operator.
    /// </summary>
    public static bool operator !=(TimeoutDuration left, TimeoutDuration right) => !left.Equals(right);

    /// <summary>
    ///     Greater than operator.
    /// </summary>
    public static bool operator >(TimeoutDuration left, TimeoutDuration right) => left.Minutes > right.Minutes;

    /// <summary>
    ///     Less than operator.
    /// </summary>
    public static bool operator <(TimeoutDuration left, TimeoutDuration right) => left.Minutes < right.Minutes;

    /// <summary>
    ///     Greater than or equal operator.
    /// </summary>
    public static bool operator >=(TimeoutDuration left, TimeoutDuration right) => left.Minutes >= right.Minutes;

    /// <summary>
    ///     Less than or equal operator.
    /// </summary>
    public static bool operator <=(TimeoutDuration left, TimeoutDuration right) => left.Minutes <= right.Minutes;

    /// <inheritdoc />
    public override string ToString() => $"{Minutes} minutes ({TimeSpan})";
}

