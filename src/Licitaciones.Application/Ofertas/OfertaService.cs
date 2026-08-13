using Licitaciones.Application.Abstractions.Time;
using Licitaciones.Application.Licitaciones;
using Licitaciones.Application.Proveedores;
using Licitaciones.Domain.Ofertas;

namespace Licitaciones.Application.Ofertas;

public sealed class OfertaService : IOfertaService
{
    private readonly IOfertaRepository _repository;
    private readonly ILicitacionRepository _licitaciones;
    private readonly IProveedorRepository _proveedores;
    private readonly IClock _clock;

    public OfertaService(
        IOfertaRepository repository,
        ILicitacionRepository licitaciones,
        IProveedorRepository proveedores,
        IClock clock)
    {
        _repository = repository;
        _licitaciones = licitaciones;
        _proveedores = proveedores;
        _clock = clock;
    }

    public async Task<OfertaResult<OfertaResponse>> CreateAsync(CrearOfertaRequest request, CancellationToken cancellationToken = default)
    {
        var licitacion = await _licitaciones.GetByIdAsync(request.LicitacionId, cancellationToken);
        if (licitacion is null || licitacion.IsDeleted) return MissingLicitacion<OfertaResponse>();
        var proveedor = await _proveedores.GetByIdAsync(request.ProveedorId, cancellationToken);
        if (proveedor is null || proveedor.IsDeleted) return MissingProveedor<OfertaResponse>();
        if (await _repository.ExistsAsync(request.LicitacionId, request.ProveedorId, cancellationToken: cancellationToken))
        {
            return Duplicate<OfertaResponse>();
        }

        try
        {
            var oferta = Oferta.Create(licitacion, request.ProveedorId, request.MontoOfertadoCrc, _clock.UtcNow);
            await _repository.AddAsync(oferta, cancellationToken);
            return await SaveAsync(oferta, cancellationToken);
        }
        catch (OfertaValidationException exception)
        {
            return Validation<OfertaResponse>(exception);
        }
    }

    public async Task<OfertaResult<OfertaResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var oferta = await _repository.GetByIdAsync(id, cancellationToken);
        return oferta is null ? NotFound<OfertaResponse>() : OfertaResult.Success(OfertaResponse.FromDomain(oferta));
    }

    public async Task<OfertaResult<OfertaPage>> ListAsync(OfertaQuery query, CancellationToken cancellationToken = default) =>
        OfertaResult.Success(await _repository.ListAsync(query, cancellationToken));

    public async Task<OfertaResult<OfertaResponse>> UpdateAsync(Guid id, ActualizarOfertaRequest request, CancellationToken cancellationToken = default)
    {
        var oferta = await _repository.GetByIdAsync(id, cancellationToken);
        if (oferta is null) return NotFound<OfertaResponse>();
        if (request.Version.HasValue && request.Version != oferta.Version) return Concurrency<OfertaResponse>();
        var licitacion = await _licitaciones.GetByIdAsync(oferta.LicitacionId, cancellationToken);
        if (licitacion is null || licitacion.IsDeleted) return MissingLicitacion<OfertaResponse>();

        try
        {
            oferta.UpdateAmount(licitacion, request.MontoOfertadoCrc, _clock.UtcNow);
            return await SaveAsync(oferta, cancellationToken);
        }
        catch (OfertaValidationException exception)
        {
            return Validation<OfertaResponse>(exception);
        }
    }

    public async Task<OfertaResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var oferta = await _repository.GetByIdAsync(id, cancellationToken);
        if (oferta is null) return NotFound<bool>();
        var licitacion = await _licitaciones.GetByIdAsync(oferta.LicitacionId, cancellationToken);
        if (licitacion is null || licitacion.IsDeleted) return MissingLicitacion<bool>();

        try
        {
            oferta.EnsureCanDelete(licitacion, _clock.UtcNow);
            _repository.Remove(oferta);
            await _repository.SaveChangesAsync(cancellationToken);
            return OfertaResult.Success(true);
        }
        catch (OfertaValidationException exception)
        {
            return Validation<bool>(exception);
        }
        catch (OfertaConcurrencyException)
        {
            return Concurrency<bool>();
        }
    }

    public async Task<OfertaResult<MejorOfertaResponse>> GetBestAsync(Guid licitacionId, CancellationToken cancellationToken = default)
    {
        var licitacion = await _licitaciones.GetByIdAsync(licitacionId, cancellationToken);
        if (licitacion is null || licitacion.IsDeleted) return MissingLicitacion<MejorOfertaResponse>();
        var ofertas = await _repository.ListByLicitacionAsync(licitacionId, cancellationToken);
        var evaluation = EvaluadorOfertas.Evaluar(licitacion.PresupuestoCrc, ofertas);
        return OfertaResult.Success(new MejorOfertaResponse(
            evaluation.TieneOferta,
            evaluation.MejorOferta is null ? null : OfertaResponse.FromDomain(evaluation.MejorOferta),
            evaluation.AhorroCrc,
            evaluation.PorcentajeAhorro,
            evaluation.Clasificacion,
            evaluation.DescripcionClasificacion));
    }

    private async Task<OfertaResult<OfertaResponse>> SaveAsync(Oferta oferta, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.SaveChangesAsync(cancellationToken);
            return OfertaResult.Success(OfertaResponse.FromDomain(oferta));
        }
        catch (OfertaConcurrencyException)
        {
            return Concurrency<OfertaResponse>();
        }
        catch (OfertaDuplicateException)
        {
            return Duplicate<OfertaResponse>();
        }
    }

    private static OfertaResult<T> Validation<T>(OfertaValidationException exception)
    {
        var error = exception.Errors[0];
        return OfertaResult.Failure<T>(OfertaResultStatus.ValidationError, error.Code, error.Message);
    }

    private static OfertaResult<T> Duplicate<T>() => OfertaResult.Failure<T>(OfertaResultStatus.Conflict, OfertaErrors.Duplicada, "El proveedor ya presento una oferta para esta licitacion.");
    private static OfertaResult<T> NotFound<T>() => OfertaResult.Failure<T>(OfertaResultStatus.NotFound, OfertaErrors.NoEncontrada, "La oferta solicitada no existe.");
    private static OfertaResult<T> MissingLicitacion<T>() => OfertaResult.Failure<T>(OfertaResultStatus.NotFound, OfertaErrors.LicitacionNoEncontrada, "La licitacion solicitada no existe.");
    private static OfertaResult<T> MissingProveedor<T>() => OfertaResult.Failure<T>(OfertaResultStatus.NotFound, OfertaErrors.ProveedorNoEncontrado, "El proveedor solicitado no existe.");
    private static OfertaResult<T> Concurrency<T>() => OfertaResult.Failure<T>(OfertaResultStatus.ConcurrencyConflict, OfertaErrors.Concurrencia, "La oferta fue modificada por otro proceso.");
}
