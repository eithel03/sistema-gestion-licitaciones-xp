using Licitaciones.Application.Ofertas;
using Licitaciones.Domain.Ofertas;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Licitaciones.Infrastructure.Persistence.Repositories;

public sealed class OfertaRepository : IOfertaRepository
{
    private readonly LicitacionesDbContext _context;

    public OfertaRepository(LicitacionesDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Oferta oferta, CancellationToken cancellationToken = default) =>
        await _context.Ofertas.AddAsync(oferta, cancellationToken);

    public Task<Oferta?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Ofertas.SingleOrDefaultAsync(oferta => oferta.Id == id, cancellationToken);

    public Task<bool> ExistsAsync(Guid licitacionId, Guid proveedorId, Guid? excludedId = null, CancellationToken cancellationToken = default) =>
        _context.Ofertas.AnyAsync(oferta =>
            oferta.LicitacionId == licitacionId &&
            oferta.ProveedorId == proveedorId &&
            (!excludedId.HasValue || oferta.Id != excludedId.Value), cancellationToken);

    public async Task<OfertaPage> ListAsync(OfertaQuery query, CancellationToken cancellationToken = default)
    {
        var ofertas = _context.Ofertas.AsNoTracking();
        if (query.LicitacionId.HasValue) ofertas = ofertas.Where(oferta => oferta.LicitacionId == query.LicitacionId.Value);
        if (query.ProveedorId.HasValue) ofertas = ofertas.Where(oferta => oferta.ProveedorId == query.ProveedorId.Value);
        ofertas = query.Sort?.ToLowerInvariant() switch
        {
            "amount" => ofertas.OrderBy(oferta => oferta.MontoOfertadoCrc).ThenBy(oferta => oferta.FechaRegistro).ThenBy(oferta => oferta.Id),
            "amount_desc" => ofertas.OrderByDescending(oferta => oferta.MontoOfertadoCrc).ThenBy(oferta => oferta.FechaRegistro).ThenBy(oferta => oferta.Id),
            "registered_desc" => ofertas.OrderByDescending(oferta => oferta.FechaRegistro).ThenByDescending(oferta => oferta.Id),
            _ => ofertas.OrderBy(oferta => oferta.FechaRegistro).ThenBy(oferta => oferta.Id)
        };
        var total = await ofertas.CountAsync(cancellationToken);
        var items = await ofertas.Skip((query.ValidPage - 1) * query.ValidPageSize).Take(query.ValidPageSize).ToListAsync(cancellationToken);
        return new OfertaPage(items.Select(OfertaResponse.FromDomain).ToList(), total, query.ValidPage, query.ValidPageSize);
    }

    public async Task<IReadOnlyList<Oferta>> ListByLicitacionAsync(Guid licitacionId, CancellationToken cancellationToken = default) =>
        await _context.Ofertas.AsNoTracking().Where(oferta => oferta.LicitacionId == licitacionId).ToListAsync(cancellationToken);

    public void Remove(Oferta oferta) => _context.Ofertas.Remove(oferta);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new OfertaConcurrencyException();
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new OfertaDuplicateException();
        }
    }
}
