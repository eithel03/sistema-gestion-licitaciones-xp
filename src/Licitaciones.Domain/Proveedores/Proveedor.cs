namespace Licitaciones.Domain.Proveedores;

public sealed class Proveedor
{
    private Proveedor()
    {
        Id = Guid.Empty;
        Nombre = string.Empty;
        NombreNormalizado = string.Empty;
    }

    private Proveedor(Guid id, string nombre, string nombreNormalizado, DateTimeOffset createdAt)
    {
        Id = id;
        Nombre = nombre;
        NombreNormalizado = nombreNormalizado;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public Guid Id { get; private set; }

    public string Nombre { get; private set; }

    public string NombreNormalizado { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public DateTimeOffset? DeletedAt { get; private set; }

    public uint Version { get; private set; }

    public bool IsDeleted => DeletedAt.HasValue;

    public static Proveedor Create(string? nombre, DateTimeOffset createdAt)
    {
        var normalized = ValidateAndNormalize(nombre);

        return new Proveedor(Guid.NewGuid(), normalized.DisplayName, normalized.ComparisonName, createdAt);
    }

    public void Rename(string? nombre, DateTimeOffset updatedAt)
    {
        var normalized = ValidateAndNormalize(nombre);

        Nombre = normalized.DisplayName;
        NombreNormalizado = normalized.ComparisonName;
        UpdatedAt = updatedAt;
    }

    public void Retire(DateTimeOffset deletedAt)
    {
        DeletedAt = deletedAt;
        UpdatedAt = deletedAt;
    }

    private static NormalizedName ValidateAndNormalize(string? nombre)
    {
        var displayName = ProveedorNameNormalizer.NormalizeForDisplay(nombre);

        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ProveedorValidationException(
                new ProveedorValidationError(ProveedorErrors.NombreRequerido, "El nombre del proveedor es requerido."));
        }

        if (!ProveedorNameNormalizer.HasAllowedCharacters(displayName))
        {
            throw new ProveedorValidationException(
                new ProveedorValidationError(
                    ProveedorErrors.NombreCaracteresInvalidos,
                    "El nombre del proveedor solo puede contener letras, numeros, espacios, punto, coma y parentesis."));
        }

        return new NormalizedName(displayName, ProveedorNameNormalizer.NormalizeForComparison(displayName));
    }

    private sealed record NormalizedName(string DisplayName, string ComparisonName);
}
