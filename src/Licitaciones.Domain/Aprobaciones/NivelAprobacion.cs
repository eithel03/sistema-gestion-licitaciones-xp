namespace Licitaciones.Domain.Aprobaciones;

public sealed class NivelAprobacion
{
    public const int AprobadorMaxLength = 200;

    private NivelAprobacion()
    {
        Aprobador = string.Empty;
    }

    private NivelAprobacion(Guid id, decimal montoMinimoCrc, decimal? montoMaximoCrc, string aprobador, DateTimeOffset createdAt)
    {
        Id = id;
        MontoMinimoCrc = montoMinimoCrc;
        MontoMaximoCrc = montoMaximoCrc;
        Aprobador = aprobador;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public decimal MontoMinimoCrc { get; private set; }
    public decimal? MontoMaximoCrc { get; private set; }
    public string Aprobador { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public uint Version { get; private set; }
    public bool IsOpen => MontoMaximoCrc is null;

    public static NivelAprobacion Create(
        decimal montoMinimoCrc,
        decimal? montoMaximoCrc,
        string? aprobador,
        DateTimeOffset createdAt)
    {
        var normalized = Validate(montoMinimoCrc, montoMaximoCrc, aprobador);
        return new NivelAprobacion(Guid.NewGuid(), montoMinimoCrc, montoMaximoCrc, normalized, createdAt);
    }

    public void Update(decimal montoMinimoCrc, decimal? montoMaximoCrc, string? aprobador, DateTimeOffset updatedAt)
    {
        var normalized = Validate(montoMinimoCrc, montoMaximoCrc, aprobador);
        MontoMinimoCrc = montoMinimoCrc;
        MontoMaximoCrc = montoMaximoCrc;
        Aprobador = normalized;
        UpdatedAt = updatedAt;
    }

    public bool Contains(decimal montoCrc) =>
        montoCrc >= MontoMinimoCrc && (!MontoMaximoCrc.HasValue || montoCrc <= MontoMaximoCrc.Value);

    public bool Overlaps(NivelAprobacion other)
    {
        ArgumentNullException.ThrowIfNull(other);
        var thisMaximum = MontoMaximoCrc ?? decimal.MaxValue;
        var otherMaximum = other.MontoMaximoCrc ?? decimal.MaxValue;
        return MontoMinimoCrc <= otherMaximum && other.MontoMinimoCrc <= thisMaximum;
    }

    private static string Validate(decimal minimum, decimal? maximum, string? approver)
    {
        if (minimum <= 0)
        {
            throw Error(NivelAprobacionErrors.MinimoInvalido, "El monto minimo debe ser mayor que cero.");
        }

        if (maximum.HasValue && maximum.Value < minimum)
        {
            throw Error(NivelAprobacionErrors.MaximoInvalido, "El monto maximo no puede ser menor que el monto minimo.");
        }

        var normalizedApprover = approver?.Trim() ?? string.Empty;
        if (normalizedApprover.Length == 0)
        {
            throw Error(NivelAprobacionErrors.AprobadorRequerido, "El aprobador es requerido.");
        }

        if (normalizedApprover.Length > AprobadorMaxLength)
        {
            throw Error(NivelAprobacionErrors.AprobadorMuyLargo, "El aprobador no debe superar 200 caracteres.");
        }

        return normalizedApprover;
    }

    private static NivelAprobacionValidationException Error(string code, string message) =>
        new(new NivelAprobacionValidationError(code, message));
}
