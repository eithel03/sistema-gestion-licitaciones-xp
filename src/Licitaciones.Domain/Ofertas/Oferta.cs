using Licitaciones.Domain.Licitaciones;

namespace Licitaciones.Domain.Ofertas;

public sealed class Oferta
{
    private Oferta()
    {
    }

    private Oferta(Guid id, Guid licitacionId, Guid proveedorId, decimal montoOfertadoCrc, DateTimeOffset fechaRegistro)
    {
        Id = id;
        LicitacionId = licitacionId;
        ProveedorId = proveedorId;
        MontoOfertadoCrc = montoOfertadoCrc;
        FechaRegistro = fechaRegistro;
        UpdatedAt = fechaRegistro;
    }

    public Guid Id { get; private set; }
    public Guid LicitacionId { get; private set; }
    public Guid ProveedorId { get; private set; }
    public decimal MontoOfertadoCrc { get; private set; }
    public DateTimeOffset FechaRegistro { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public uint Version { get; private set; }

    public static Oferta Create(Licitacion licitacion, Guid proveedorId, decimal montoOfertadoCrc, DateTimeOffset fechaRegistro)
    {
        ArgumentNullException.ThrowIfNull(licitacion);
        ValidateProvider(proveedorId);
        ValidateMutation(licitacion, montoOfertadoCrc, fechaRegistro);
        return new Oferta(Guid.NewGuid(), licitacion.Id, proveedorId, montoOfertadoCrc, fechaRegistro);
    }

    public void UpdateAmount(Licitacion licitacion, decimal montoOfertadoCrc, DateTimeOffset updatedAt)
    {
        ArgumentNullException.ThrowIfNull(licitacion);
        EnsureSameLicitacion(licitacion);
        ValidateMutation(licitacion, montoOfertadoCrc, updatedAt);
        MontoOfertadoCrc = montoOfertadoCrc;
        UpdatedAt = updatedAt;
    }

    public void EnsureCanDelete(Licitacion licitacion, DateTimeOffset utcNow)
    {
        ArgumentNullException.ThrowIfNull(licitacion);
        EnsureSameLicitacion(licitacion);
        EnsureReceivesOffers(licitacion, utcNow);
    }

    private static void ValidateMutation(Licitacion licitacion, decimal amount, DateTimeOffset utcNow)
    {
        if (amount <= 0)
        {
            throw Error(OfertaErrors.MontoInvalido, "El monto ofertado debe ser mayor que cero.");
        }

        if (amount > licitacion.PresupuestoCrc)
        {
            throw Error(OfertaErrors.SuperaPresupuesto, "El monto ofertado no puede superar el presupuesto de la licitacion.");
        }

        EnsureReceivesOffers(licitacion, utcNow);
    }

    private static void EnsureReceivesOffers(Licitacion licitacion, DateTimeOffset utcNow)
    {
        if (licitacion.GetEstadoEfectivo(utcNow) != LicitacionEstado.Publicada || licitacion.IsDeleted)
        {
            throw Error(OfertaErrors.LicitacionNoRecibeOfertas, "La licitacion no esta disponible para recibir ofertas.");
        }
    }

    private void EnsureSameLicitacion(Licitacion licitacion)
    {
        if (licitacion.Id != LicitacionId)
        {
            throw new ArgumentException("La licitacion no corresponde a la oferta.", nameof(licitacion));
        }
    }

    private static void ValidateProvider(Guid proveedorId)
    {
        if (proveedorId == Guid.Empty)
        {
            throw Error(OfertaErrors.ProveedorInvalido, "El proveedor es requerido.");
        }
    }

    private static OfertaValidationException Error(string code, string message) =>
        new(new OfertaValidationError(code, message));
}
