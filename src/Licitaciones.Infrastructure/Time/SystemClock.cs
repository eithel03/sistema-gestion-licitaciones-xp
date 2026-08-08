using Licitaciones.Application.Abstractions.Time;

namespace Licitaciones.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
