using Licitaciones.Application.Aprobaciones;
using Licitaciones.Domain.Aprobaciones;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Licitaciones.Infrastructure.Persistence.Repositories;

public sealed class NivelAprobacionRepository : INivelAprobacionRepository
{
    private readonly LicitacionesDbContext _context;

    public NivelAprobacionRepository(LicitacionesDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(NivelAprobacion nivel, CancellationToken cancellationToken = default) =>
        await _context.NivelesAprobacion.AddAsync(nivel, cancellationToken);

    public Task<NivelAprobacion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.NivelesAprobacion.SingleOrDefaultAsync(nivel => nivel.Id == id, cancellationToken);

    public async Task<NivelAprobacionPage> ListAsync(NivelAprobacionQuery query, CancellationToken cancellationToken = default)
    {
        var levels = _context.NivelesAprobacion.AsNoTracking().OrderBy(nivel => nivel.MontoMinimoCrc);
        var total = await levels.CountAsync(cancellationToken);
        var items = await levels.Skip((query.ValidPage - 1) * query.ValidPageSize).Take(query.ValidPageSize).ToListAsync(cancellationToken);
        return new NivelAprobacionPage(items.Select(NivelAprobacionResponse.FromDomain).ToList(), total, query.ValidPage, query.ValidPageSize);
    }

    public Task<bool> HasOverlapAsync(decimal minimum, decimal? maximum, Guid? excludedId = null, CancellationToken cancellationToken = default) =>
        _context.NivelesAprobacion.AnyAsync(nivel =>
            (!excludedId.HasValue || nivel.Id != excludedId.Value) &&
            (!maximum.HasValue || nivel.MontoMinimoCrc <= maximum.Value) &&
            (!nivel.MontoMaximoCrc.HasValue || minimum <= nivel.MontoMaximoCrc.Value), cancellationToken);

    public Task<bool> HasOpenRangeAsync(Guid? excludedId = null, CancellationToken cancellationToken = default) =>
        _context.NivelesAprobacion.AnyAsync(nivel =>
            nivel.MontoMaximoCrc == null && (!excludedId.HasValue || nivel.Id != excludedId.Value), cancellationToken);

    public Task<NivelAprobacion?> FindByAmountAsync(decimal amount, CancellationToken cancellationToken = default) =>
        _context.NivelesAprobacion.AsNoTracking()
            .Where(nivel => nivel.MontoMinimoCrc <= amount && (!nivel.MontoMaximoCrc.HasValue || amount <= nivel.MontoMaximoCrc.Value))
            .OrderBy(nivel => nivel.MontoMinimoCrc)
            .FirstOrDefaultAsync(cancellationToken);

    public void Remove(NivelAprobacion nivel) => _context.NivelesAprobacion.Remove(nivel);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new NivelAprobacionConcurrencyException();
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException postgres &&
            (postgres.SqlState == PostgresErrorCodes.UniqueViolation || postgres.SqlState == PostgresErrorCodes.ExclusionViolation))
        {
            throw new NivelAprobacionRangeConflictException();
        }
    }
}
