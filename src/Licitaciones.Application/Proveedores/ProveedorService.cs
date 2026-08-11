using Licitaciones.Application.Abstractions.Time;
using Licitaciones.Domain.Proveedores;

namespace Licitaciones.Application.Proveedores;

public sealed class ProveedorService : IProveedorService
{
    private readonly IProveedorRepository _repository;
    private readonly IClock _clock;

    public ProveedorService(IProveedorRepository repository, IClock clock)
    {
        _repository = repository;
        _clock = clock;
    }

    public async Task<ProveedorResult<ProveedorResponse>> CreateAsync(
        CrearProveedorRequest request,
        CancellationToken cancellationToken = default)
    {
        var proveedorResult = CreateProveedor(request.Nombre);

        if (!proveedorResult.Succeeded)
        {
            return ProveedorResult.Failure<ProveedorResponse>(
                proveedorResult.Status,
                proveedorResult.ErrorCode!,
                proveedorResult.ErrorMessage!);
        }

        var proveedor = proveedorResult.Value!;
        if (await _repository.ExistsByNormalizedNameAsync(proveedor.NombreNormalizado, cancellationToken: cancellationToken))
        {
            return DuplicateResult<ProveedorResponse>();
        }

        await _repository.AddAsync(proveedor, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return ProveedorResult.Success(ProveedorResponse.FromDomain(proveedor));
    }

    public async Task<ProveedorResult<ProveedorResponse>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var proveedor = await _repository.GetByIdAsync(id, cancellationToken);

        return proveedor is null || proveedor.DeletedAt is not null
            ? NotFoundResult<ProveedorResponse>()
            : ProveedorResult.Success(ProveedorResponse.FromDomain(proveedor));
    }

    public async Task<ProveedorResult<ProveedorPage>> ListAsync(
        ProveedorQuery query,
        CancellationToken cancellationToken = default)
    {
        var page = await _repository.ListAsync(query, cancellationToken);

        return ProveedorResult.Success(page);
    }

    public async Task<ProveedorResult<ProveedorResponse>> UpdateAsync(
        Guid id,
        ActualizarProveedorRequest request,
        CancellationToken cancellationToken = default)
    {
        var proveedor = await _repository.GetByIdAsync(id, cancellationToken);

        if (proveedor is null || proveedor.DeletedAt is not null)
        {
            return NotFoundResult<ProveedorResponse>();
        }

        var normalizedNameResult = NormalizeName(request.Nombre);

        if (!normalizedNameResult.Succeeded)
        {
            return ProveedorResult.Failure<ProveedorResponse>(
                normalizedNameResult.Status,
                normalizedNameResult.ErrorCode!,
                normalizedNameResult.ErrorMessage!);
        }

        var normalizedName = normalizedNameResult.Value!;
        if (await _repository.ExistsByNormalizedNameAsync(normalizedName, id, cancellationToken))
        {
            return DuplicateResult<ProveedorResponse>();
        }

        try
        {
            proveedor.Rename(request.Nombre, _clock.UtcNow);
        }
        catch (ProveedorValidationException exception)
        {
            return ValidationFailure<ProveedorResponse>(exception);
        }

        await _repository.SaveChangesAsync(cancellationToken);

        return ProveedorResult.Success(ProveedorResponse.FromDomain(proveedor));
    }

    public async Task<ProveedorResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var proveedor = await _repository.GetByIdAsync(id, cancellationToken);

        if (proveedor is null || proveedor.DeletedAt is not null)
        {
            return NotFoundResult<bool>();
        }

        proveedor.Retire(_clock.UtcNow);
        await _repository.SaveChangesAsync(cancellationToken);

        return ProveedorResult.Success(true);
    }

    private ProveedorResult<Proveedor> CreateProveedor(string? nombre)
    {
        try
        {
            return ProveedorResult.Success(Proveedor.Create(nombre, _clock.UtcNow));
        }
        catch (ProveedorValidationException exception)
        {
            return ValidationFailure<Proveedor>(exception);
        }
    }

    private static ProveedorResult<string> NormalizeName(string? nombre)
    {
        try
        {
            return ProveedorResult.Success(Proveedor.Create(nombre, DateTimeOffset.UnixEpoch).NombreNormalizado);
        }
        catch (ProveedorValidationException exception)
        {
            return ValidationFailure<string>(exception);
        }
    }

    private static ProveedorResult<T> ValidationFailure<T>(ProveedorValidationException exception)
    {
        var error = exception.Errors.First();

        return ProveedorResult.Failure<T>(
            ProveedorResultStatus.ValidationError,
            error.Code,
            error.Message);
    }

    private static ProveedorResult<T> DuplicateResult<T>()
    {
        return ProveedorResult.Failure<T>(
            ProveedorResultStatus.Conflict,
            ProveedorErrors.NombreDuplicado,
            "Ya existe un proveedor con un nombre equivalente.");
    }

    private static ProveedorResult<T> NotFoundResult<T>()
    {
        return ProveedorResult.Failure<T>(
            ProveedorResultStatus.NotFound,
            ProveedorErrors.NoEncontrado,
            "El proveedor solicitado no existe.");
    }
}
