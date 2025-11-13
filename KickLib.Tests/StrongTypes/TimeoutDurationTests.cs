using FluentAssertions;
using KickLib.Models.v1.Moderation;

namespace KickLib.Tests.StrongTypes;

public class TimeoutDurationTests
{
    [Fact]
    public void Constructor_WithValidMinutes_CreatesInstance()
    {
        // Arrange & Act
        var duration = new TimeoutDuration(60);

        // Assert
        duration.Minutes.Should().Be(60);
    }

    [Fact]
    public void Constructor_WithMinimumValue_CreatesInstance()
    {
        // Arrange & Act
        var duration = new TimeoutDuration(1);

        // Assert
        duration.Minutes.Should().Be(1);
    }

    [Fact]
    public void Constructor_WithMaximumValue_CreatesInstance()
    {
        // Arrange & Act
        var duration = new TimeoutDuration(10080);

        // Assert
        duration.Minutes.Should().Be(10080);
    }

    [Fact]
    public void Constructor_WithZero_ThrowsArgumentOutOfRangeException()
    {
        // Act
        var act = () => new TimeoutDuration(0);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WithNegativeValue_ThrowsArgumentOutOfRangeException()
    {
        // Act
        var act = () => new TimeoutDuration(-5);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_WithValueAboveMaximum_ThrowsArgumentOutOfRangeException()
    {
        // Act
        var act = () => new TimeoutDuration(10081);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Min_ReturnsOneMinute()
    {
        // Assert
        TimeoutDuration.Min.Minutes.Should().Be(1);
    }

    [Fact]
    public void Max_Returns10080Minutes()
    {
        // Assert
        TimeoutDuration.Max.Minutes.Should().Be(10080);
    }

    [Fact]
    public void TimeSpan_ReturnsCorrectTimeSpan()
    {
        // Arrange
        var duration = new TimeoutDuration(120);

        // Act
        var timeSpan = duration.TimeSpan;

        // Assert
        timeSpan.Should().Be(TimeSpan.FromMinutes(120));
    }

    [Fact]
    public void FromTimeSpan_WithValidTimeSpan_CreatesInstance()
    {
        // Arrange
        var timeSpan = TimeSpan.FromHours(2);

        // Act
        var duration = TimeoutDuration.FromTimeSpan(timeSpan);

        // Assert
        duration.Minutes.Should().Be(120);
    }

    [Fact]
    public void FromTimeSpan_WithMinimumTimeSpan_CreatesInstance()
    {
        // Arrange
        var timeSpan = TimeSpan.FromMinutes(1);

        // Act
        var duration = TimeoutDuration.FromTimeSpan(timeSpan);

        // Assert
        duration.Minutes.Should().Be(1);
    }

    [Fact]
    public void FromTimeSpan_WithMaximumTimeSpan_CreatesInstance()
    {
        // Arrange
        var timeSpan = TimeSpan.FromMinutes(10080);

        // Act
        var duration = TimeoutDuration.FromTimeSpan(timeSpan);

        // Assert
        duration.Minutes.Should().Be(10080);
    }

    [Fact]
    public void FromTimeSpan_WithInvalidTimeSpan_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var timeSpan = TimeSpan.FromDays(8);

        // Act
        var act = () => TimeoutDuration.FromTimeSpan(timeSpan);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void FromHours_WithValidHours_CreatesInstance()
    {
        // Act
        var duration = TimeoutDuration.FromHours(2.5);

        // Assert
        duration.Minutes.Should().Be(150);
    }

    [Fact]
    public void FromHours_WithOneHour_CreatesInstance()
    {
        // Act
        var duration = TimeoutDuration.FromHours(1);

        // Assert
        duration.Minutes.Should().Be(60);
    }

    [Fact]
    public void FromHours_WithInvalidHours_ThrowsArgumentOutOfRangeException()
    {
        // Act
        var act = () => TimeoutDuration.FromHours(200);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void FromDays_WithValidDays_CreatesInstance()
    {
        // Act
        var duration = TimeoutDuration.FromDays(3);

        // Assert
        duration.Minutes.Should().Be(4320);
    }

    [Fact]
    public void FromDays_WithSevenDays_CreatesMaxDuration()
    {
        // Act
        var duration = TimeoutDuration.FromDays(7);

        // Assert
        duration.Minutes.Should().Be(10080);
    }

    [Fact]
    public void FromDays_WithInvalidDays_ThrowsArgumentOutOfRangeException()
    {
        // Act
        var act = () => TimeoutDuration.FromDays(8);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void OneMinute_ReturnsOneMinuteDuration()
    {
        // Act
        var duration = TimeoutDuration.OneMinute;

        // Assert
        duration.Minutes.Should().Be(1);
    }

    [Fact]
    public void FiveMinutes_ReturnsFiveMinutesDuration()
    {
        // Act
        var duration = TimeoutDuration.FiveMinutes;

        // Assert
        duration.Minutes.Should().Be(5);
    }

    [Fact]
    public void ThirtyMinutes_ReturnsThirtyMinutesDuration()
    {
        // Act
        var duration = TimeoutDuration.ThirtyMinutes;

        // Assert
        duration.Minutes.Should().Be(30);
    }

    [Fact]
    public void OneHour_ReturnsSixtyMinutesDuration()
    {
        // Act
        var duration = TimeoutDuration.OneHour;

        // Assert
        duration.Minutes.Should().Be(60);
    }

    [Fact]
    public void OneDay_Returns1440MinutesDuration()
    {
        // Act
        var duration = TimeoutDuration.OneDay;

        // Assert
        duration.Minutes.Should().Be(1440);
    }

    [Fact]
    public void ImplicitConversion_ToInt_ReturnsMinutes()
    {
        // Arrange
        var duration = new TimeoutDuration(120);

        // Act
        int minutes = duration;

        // Assert
        minutes.Should().Be(120);
    }

    [Fact]
    public void ImplicitConversion_ToTimeSpan_ReturnsTimeSpan()
    {
        // Arrange
        var duration = new TimeoutDuration(120);

        // Act
        TimeSpan timeSpan = duration;

        // Assert
        timeSpan.Should().Be(TimeSpan.FromMinutes(120));
    }

    [Fact]
    public void ExplicitConversion_FromInt_CreatesInstance()
    {
        // Act
        var duration = (TimeoutDuration)120;

        // Assert
        duration.Minutes.Should().Be(120);
    }

    [Fact]
    public void ExplicitConversion_FromInvalidInt_ThrowsArgumentOutOfRangeException()
    {
        // Act
        var act = () => (TimeoutDuration)20000;

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ExplicitConversion_FromTimeSpan_CreatesInstance()
    {
        // Arrange
        var timeSpan = TimeSpan.FromHours(2);

        // Act
        var duration = (TimeoutDuration)timeSpan;

        // Assert
        duration.Minutes.Should().Be(120);
    }

    [Fact]
    public void ExplicitConversion_FromInvalidTimeSpan_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var timeSpan = TimeSpan.FromDays(10);

        // Act
        var act = () => (TimeoutDuration)timeSpan;

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Equals_WithSameDuration_ReturnsTrue()
    {
        // Arrange
        var duration1 = new TimeoutDuration(120);
        var duration2 = new TimeoutDuration(120);

        // Assert
        duration1.Equals(duration2).Should().BeTrue();
        (duration1 == duration2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentDuration_ReturnsFalse()
    {
        // Arrange
        var duration1 = new TimeoutDuration(120);
        var duration2 = new TimeoutDuration(60);

        // Assert
        duration1.Equals(duration2).Should().BeFalse();
        (duration1 != duration2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithObject_ReturnsCorrectResult()
    {
        // Arrange
        var duration1 = new TimeoutDuration(120);
        object duration2 = new TimeoutDuration(120);
        object notDuration = 120;

        // Assert
        duration1.Equals(duration2).Should().BeTrue();
        duration1.Equals(notDuration).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WithSameDuration_ReturnsSameHashCode()
    {
        // Arrange
        var duration1 = new TimeoutDuration(120);
        var duration2 = new TimeoutDuration(120);

        // Assert
        duration1.GetHashCode().Should().Be(duration2.GetHashCode());
    }

    [Fact]
    public void CompareTo_WithSmallerDuration_ReturnsPositive()
    {
        // Arrange
        var duration1 = new TimeoutDuration(120);
        var duration2 = new TimeoutDuration(60);

        // Act
        var result = duration1.CompareTo(duration2);

        // Assert
        result.Should().BePositive();
    }

    [Fact]
    public void CompareTo_WithLargerDuration_ReturnsNegative()
    {
        // Arrange
        var duration1 = new TimeoutDuration(60);
        var duration2 = new TimeoutDuration(120);

        // Act
        var result = duration1.CompareTo(duration2);

        // Assert
        result.Should().BeNegative();
    }

    [Fact]
    public void CompareTo_WithEqualDuration_ReturnsZero()
    {
        // Arrange
        var duration1 = new TimeoutDuration(120);
        var duration2 = new TimeoutDuration(120);

        // Act
        var result = duration1.CompareTo(duration2);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void GreaterThan_Operator_ReturnsCorrectResult()
    {
        // Arrange
        var duration1 = new TimeoutDuration(120);
        var duration2 = new TimeoutDuration(60);

        // Assert
        (duration1 > duration2).Should().BeTrue();
        (duration2 > duration1).Should().BeFalse();
    }

    [Fact]
    public void LessThan_Operator_ReturnsCorrectResult()
    {
        // Arrange
        var duration1 = new TimeoutDuration(60);
        var duration2 = new TimeoutDuration(120);

        // Assert
        (duration1 < duration2).Should().BeTrue();
        (duration2 < duration1).Should().BeFalse();
    }

    [Fact]
    public void GreaterThanOrEqual_Operator_ReturnsCorrectResult()
    {
        // Arrange
        var duration1 = new TimeoutDuration(120);
        var duration2 = new TimeoutDuration(60);
        var duration3 = new TimeoutDuration(120);

        // Assert
        (duration1 >= duration2).Should().BeTrue();
        (duration1 >= duration3).Should().BeTrue();
        (duration2 >= duration1).Should().BeFalse();
    }

    [Fact]
    public void LessThanOrEqual_Operator_ReturnsCorrectResult()
    {
        // Arrange
        var duration1 = new TimeoutDuration(60);
        var duration2 = new TimeoutDuration(120);
        var duration3 = new TimeoutDuration(60);

        // Assert
        (duration1 <= duration2).Should().BeTrue();
        (duration1 <= duration3).Should().BeTrue();
        (duration2 <= duration1).Should().BeFalse();
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var duration = new TimeoutDuration(120);

        // Act
        var result = duration.ToString();

        // Assert
        result.Should().Contain("120 minutes");
        result.Should().Contain("02:00:00");
    }

    [Fact]
    public void BoundaryTest_MinPlusOne_IsValid()
    {
        // Act
        var duration = new TimeoutDuration(2);

        // Assert
        duration.Minutes.Should().Be(2);
    }

    [Fact]
    public void BoundaryTest_MaxMinusOne_IsValid()
    {
        // Act
        var duration = new TimeoutDuration(10079);

        // Assert
        duration.Minutes.Should().Be(10079);
    }

    [Fact]
    public void PredefinedDurations_AreAllValid()
    {
        // Assert all predefined durations are within valid range
        TimeoutDuration.OneMinute.Minutes.Should().BeInRange(1, 10080);
        TimeoutDuration.FiveMinutes.Minutes.Should().BeInRange(1, 10080);
        TimeoutDuration.ThirtyMinutes.Minutes.Should().BeInRange(1, 10080);
        TimeoutDuration.OneHour.Minutes.Should().BeInRange(1, 10080);
        TimeoutDuration.OneDay.Minutes.Should().BeInRange(1, 10080);
    }

    [Fact]
    public void FromTimeSpan_WithFractionalMinutes_TruncatesCorrectly()
    {
        // Arrange - 2.5 minutes = 150 seconds
        var timeSpan = TimeSpan.FromSeconds(150);

        // Act
        var duration = TimeoutDuration.FromTimeSpan(timeSpan);

        // Assert - Should truncate to 2 minutes
        duration.Minutes.Should().Be(2);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(60)]
    [InlineData(120)]
    [InlineData(1440)]
    [InlineData(10080)]
    public void Constructor_WithVariousValidValues_CreatesInstance(int minutes)
    {
        // Act
        var duration = new TimeoutDuration(minutes);

        // Assert
        duration.Minutes.Should().Be(minutes);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(10081)]
    [InlineData(20000)]
    public void Constructor_WithVariousInvalidValues_ThrowsException(int minutes)
    {
        // Act
        var act = () => new TimeoutDuration(minutes);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}