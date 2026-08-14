using Licitaciones.Application.TiposCambio;
using Licitaciones.Domain.TiposCambio;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Licitaciones.Infrastructure.Persistence.Repositories;

public sealed class TipoCambioRepository : ITipoCambioRepository
{
    private readonly LicitacionesDbContext _context;

    public TipoCambioRepository(LicitacionesDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TipoCambio tipoCambio, CancellationToken cancellationToken = default) =>
        await Set.AddAsync(tipoCambio, cancellationToken);

    public Task<TipoCambio?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Set.SingleOrDefaultAsync(tipoCambio => tipoCambio.Id == id, cancellationToken);

    public Task<TipoCambio?> GetActiveAsync(CancellationToken cancellationToken = default) =>
        Set.AsNoTracking().SingleOrDefaultAsync(tipoCambio => tipoCambio.Activo, cancellationToken);

    public async Task<TipoCambioPage> ListAsync(TipoCambioQuery query, CancellationToken cancellationToken = default)
    {
        var tiposCambio = Set.AsNoTracking()
            .OrderByDescending(tipoCambio => tipoCambio.Activo)
            .ThenByDescending(tipoCambio => tipoCambio.Fecha);
        var total = await tiposCambio.CountAsync(cancellationToken);
        var items = await tiposCambio.Skip((query.ValidPage - 1) * query.ValidPageSize).Take(query.ValidPageSize).ToListAsync(cancellationToken);
        return new TipoCambioPage(items.Select(TipoCambioResponse.FromDomain).ToList(), total, query.ValidPage, query.ValidPageSize);
    }

    public async Task DeactivateAllExceptAsync(Guid activeId, DateTimeOffset updatedAt, CancellationToken cancellationToken = default)
    {
        await Set
            .Where(tipoCambio => tipoCambio.Id != activeId && tipoCambio.Activo)
            .ExecuteUpdateAsync(updates => updates
                .SetProperty(tipoCambio => tipoCambio.Activo, false)
                .SetProperty(tipoCambio => tipoCambio.UpdatedAt, updatedAt),
                cancellationToken);
    }

    public void Remove(TipoCambio tipoCambio) => Set.Remove(tipoCambio);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new TipoCambioConcurrencyException();
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException postgres &&
            postgres.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new TipoCambioActiveConflictException();
        }
    }

    private DbSet<TipoCambio> Set => _context.Set<TipoCambio>();
}
