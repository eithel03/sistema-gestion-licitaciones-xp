namespace Licitaciones.Domain.TiposCambio;

public sealed class TipoCambio
{
    private TipoCambio()
    {
    }

    private TipoCambio(Guid id, DateOnly fecha, decimal crcPorUsd, DateTimeOffset createdAt)
    {
        Id = id;
        Fecha = fecha;
        CrcPorUsd = crcPorUsd;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public DateOnly Fecha { get; private set; }
    public decimal CrcPorUsd { get; private set; }
    public bool Activo { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public uint Version { get; private set; }

    public static TipoCambio Create(DateOnly fecha, decimal crcPorUsd, DateTimeOffset createdAt)
    {
        Validate(fecha, crcPorUsd);
        return new TipoCambio(Guid.NewGuid(), fecha, crcPorUsd, createdAt);
    }

    public void Update(DateOnly fecha, decimal crcPorUsd, DateTimeOffset updatedAt)
    {
        Validate(fecha, crcPorUsd);
        Fecha = fecha;
        CrcPorUsd = crcPorUsd;
        UpdatedAt = updatedAt;
    }

    public void Activate(DateTimeOffset updatedAt)
    {
        Activo = true;
        UpdatedAt = updatedAt;
    }

    public void Deactivate(DateTimeOffset updatedAt)
    {
        Activo = false;
        UpdatedAt = updatedAt;
    }

    public decimal ConvertCrcToUsd(decimal amountCrc) => Math.Round(amountCrc / CrcPorUsd, 2, MidpointRounding.AwayFromZero);

    private static void Validate(DateOnly fecha, decimal crcPorUsd)
    {
        if (fecha == default)
        {
            throw Error(TipoCambioErrors.FechaInvalida, "La fecha del tipo de cambio es requerida.");
        }

        if (crcPorUsd <= 0)
        {
            throw Error(TipoCambioErrors.ValorInvalido, "El valor del tipo de cambio debe ser mayor que cero.");
        }
    }

    private static TipoCambioValidationException Error(string code, string message) =>
        new(new TipoCambioValidationError(code, message));
}
