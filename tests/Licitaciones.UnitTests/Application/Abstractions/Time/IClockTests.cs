using Licitaciones.Application.Abstractions.Time;

namespace Licitaciones.UnitTests.Application.Abstractions.Time;

public sealed class IClockTests
{
    [Fact]
    public void ClockCanBeReplacedByFixedImplementationInTests()
    {
        var expectedNow = new DateTimeOffset(2026, 8, 8, 12, 0, 0, TimeSpan.Zero);
        IClock clock = new FixedClock(expectedNow);

        Assert.Equal(expectedNow, clock.UtcNow);
    }

    private sealed class FixedClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow { get; } = utcNow;
    }
}
