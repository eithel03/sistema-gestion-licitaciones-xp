
namespace Licitaciones.Domain.Licitaciones;

public sealed class Licitacion
{
    public const int CodigoMaxLength = 50;
    public const int TituloMaxLength = 200;

    private Licitacion()
    {
        Id = Guid.Empty;
        Codigo = string.Empty;
        CodigoNormalizado = string.Empty;
        Titulo = string.Empty;
    }

    private Licitacion(Guid id, string codigo, string codigoNormalizado, string titulo, decimal presupuestoCrc, DateTimeOffset fechaCierreUtc, DateTimeOffset createdAt)
    {
        Id = id;
        Codigo = codigo;
        CodigoNormalizado = codigoNormalizado;
        Titulo = titulo;
        PresupuestoCrc = presupuestoCrc;
        FechaCierreUtc = fechaCierreUtc;
        Estado = LicitacionEstado.Borrador;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public string Codigo { get; private set; }
    public string CodigoNormalizado { get; private set; }
    public string Titulo { get; private set; }
    public decimal PresupuestoCrc { get; private set; }
    public DateTimeOffset FechaCierreUtc { get; private set; }
    public LicitacionEstado Estado { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public DateTimeOffset? PublishedAt { get; private set; }
    public DateTimeOffset? ClosedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public uint Version { get; private set; }
    public bool IsDeleted => DeletedAt.HasValue;

    public static Licitacion Create(string? codigo, string? titulo, decimal presupuestoCrc, DateTimeOffset fechaCierreUtc, DateTimeOffset createdAt)
    {
        var normalized = Validate(codigo, titulo, presupuestoCrc, fechaCierreUtc, createdAt);
        return new Licitacion(Guid.NewGuid(), normalized.Codigo, normalized.CodigoNormalizado, normalized.Titulo, presupuestoCrc, fechaCierreUtc, createdAt);
    }

    public void Update(string? codigo, string? titulo, decimal presupuestoCrc, DateTimeOffset fechaCierreUtc, DateTimeOffset updatedAt)
    {
        EnsureDraftForMutation();
        var normalized = Validate(codigo, titulo, presupuestoCrc, fechaCierreUtc, updatedAt);
        Codigo = normalized.Codigo;
        CodigoNormalizado = normalized.CodigoNormalizado;
        Titulo = normalized.Titulo;
        PresupuestoCrc = presupuestoCrc;
        FechaCierreUtc = fechaCierreUtc;
        UpdatedAt = updatedAt;
    }
    public void Retire(DateTimeOffset deletedAt)
    {
        EnsureDraftForMutation();
        DeletedAt = deletedAt;
        UpdatedAt = deletedAt;
    }

    public void Publish(DateTimeOffset publishedAt)
    {
        if (Estado != LicitacionEstado.Borrador || IsDeleted || FechaCierreUtc <= publishedAt)
        {
            throw InvalidTransition("Solo una licitacion en borrador y vigente puede publicarse.");
        }

        Estado = LicitacionEstado.Publicada;
        PublishedAt = publishedAt;
        UpdatedAt = publishedAt;
    }

    public void Close(DateTimeOffset closedAt)
    {
        if (Estado is not (LicitacionEstado.Borrador or LicitacionEstado.Publicada) || IsDeleted)
        {
            throw InvalidTransition("Solo una licitacion en borrador o publicada puede cerrarse.");
        }

        Estado = LicitacionEstado.Cerrada;
        ClosedAt = closedAt;
        UpdatedAt = closedAt;
    }

    public void ChangeEstado(LicitacionEstado estado, DateTimeOffset changedAt)
    {
        if (estado == Estado)
        {
            return;
        }

        switch (estado)
        {
            case LicitacionEstado.Borrador:
                throw InvalidTransition("No se puede regresar una licitacion a borrador.");
            case LicitacionEstado.Publicada:
                Publish(changedAt);
                break;
            case LicitacionEstado.Cerrada:
                Close(changedAt);
                break;
            default:
                throw InvalidTransition("El estado solicitado no es valido.");
        }
    }

    public LicitacionEstado GetEstadoEfectivo(DateTimeOffset utcNow)
    {
        return Estado == LicitacionEstado.Publicada && FechaCierreUtc <= utcNow
            ? LicitacionEstado.Cerrada
            : Estado;
    }
    private static NormalizedInput Validate(string? codigo, string? titulo, decimal presupuestoCrc, DateTimeOffset fechaCierreUtc, DateTimeOffset utcNow)
    {
        var displayCode = LicitacionCodeNormalizer.NormalizeForDisplay(codigo);
        if (string.IsNullOrWhiteSpace(displayCode))
        {
            throw new LicitacionValidationException(new LicitacionValidationError(LicitacionErrors.CodigoRequerido, "El codigo de la licitacion es requerido."));
        }

        if (displayCode.Length > CodigoMaxLength)
        {
            throw new LicitacionValidationException(new LicitacionValidationError(LicitacionErrors.CodigoLongitudMaxima, "El codigo no debe superar 50 caracteres."));
        }

        if (!LicitacionCodeNormalizer.HasAllowedCharacters(displayCode))
        {
            throw new LicitacionValidationException(new LicitacionValidationError(LicitacionErrors.CodigoCaracteresInvalidos, "El codigo solo puede contener letras, numeros, espacios y guion."));
        }

        var title = string.IsNullOrWhiteSpace(titulo) ? string.Empty : titulo.Trim();
        if (title.Length == 0)
        {
            throw new LicitacionValidationException(new LicitacionValidationError(LicitacionErrors.TituloRequerido, "El titulo de la licitacion es requerido."));
        }
        if (title.Length > TituloMaxLength)
        {
            throw new LicitacionValidationException(new LicitacionValidationError(LicitacionErrors.TituloLongitudMaxima, "El titulo no debe superar 200 caracteres."));
        }

        if (presupuestoCrc <= 0)
        {
            throw new LicitacionValidationException(new LicitacionValidationError(LicitacionErrors.PresupuestoInvalido, "El presupuesto debe ser mayor que cero."));
        }

        if (fechaCierreUtc <= utcNow)
        {
            throw new LicitacionValidationException(new LicitacionValidationError(LicitacionErrors.FechaCierreInvalida, "La fecha de cierre debe ser posterior al momento actual."));
        }

        return new NormalizedInput(displayCode, LicitacionCodeNormalizer.NormalizeForComparison(displayCode), title);
    }

    private void EnsureDraftForMutation()
    {
        if (Estado != LicitacionEstado.Borrador || IsDeleted)
        {
            throw new LicitacionValidationException(new LicitacionValidationError(LicitacionErrors.EdicionNoPermitida, "Solo una licitacion en borrador puede modificarse o retirarse."));
        }
    }

    private static LicitacionValidationException InvalidTransition(string message)
    {
        return new LicitacionValidationException(new LicitacionValidationError(LicitacionErrors.TransicionInvalida, message));
    }

    private sealed record NormalizedInput(string Codigo, string CodigoNormalizado, string Titulo);
}
