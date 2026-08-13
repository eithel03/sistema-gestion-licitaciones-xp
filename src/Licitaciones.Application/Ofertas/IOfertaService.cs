using Licitaciones.Domain.Ofertas;

namespace Licitaciones.Application.Ofertas;

public interface IOfertaRepository
{
    Task AddAsync(Oferta oferta, CancellationToken cancellationToken = default);
    Task<Oferta?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid licitacionId, Guid proveedorId, Guid? excludedId = null, CancellationToken cancellationToken = default);
    Task<OfertaPage> ListAsync(OfertaQuery query, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Oferta>> ListByLicitacionAsync(Guid licitacionId, CancellationToken cancellationToken = default);
    void Remove(Oferta oferta);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IOfertaService
{
    Task<OfertaResult<OfertaResponse>> CreateAsync(CrearOfertaRequest request, CancellationToken cancellationToken = default);
    Task<OfertaResult<OfertaResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OfertaResult<OfertaPage>> ListAsync(OfertaQuery query, CancellationToken cancellationToken = default);
    Task<OfertaResult<OfertaResponse>> UpdateAsync(Guid id, ActualizarOfertaRequest request, CancellationToken cancellationToken = default);
    Task<OfertaResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OfertaResult<MejorOfertaResponse>> GetBestAsync(Guid licitacionId, CancellationToken cancellationToken = default);
}
