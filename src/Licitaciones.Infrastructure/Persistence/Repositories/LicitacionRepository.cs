using Licitaciones.Application.Licitaciones;
using Licitaciones.Domain.Licitaciones;
using Microsoft.EntityFrameworkCore;

namespace Licitaciones.Infrastructure.Persistence.Repositories;

public sealed class LicitacionRepository : ILicitacionRepository
{
    private readonly LicitacionesDbContext _context;

    public LicitacionRepository(LicitacionesDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Licitacion licitacion, CancellationToken cancellationToken = default)
    {
        await _context.Licitaciones.AddAsync(licitacion, cancellationToken);
    }

    public Task<Licitacion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Licitaciones.IgnoreQueryFilters().SingleOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public Task<bool> ExistsByNormalizedCodeAsync(string normalizedCode, Guid? excludedId = null, CancellationToken cancellationToken = default)
    {
        return _context.Licitaciones.AnyAsync(l => l.CodigoNormalizado == normalizedCode, cancellationToken);
    }
    public async Task<LicitacionPage> ListAsync(LicitacionQuery query, DateTimeOffset utcNow, CancellationToken cancellationToken = default)
    {
        var licitaciones = _context.Licitaciones.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            licitaciones = licitaciones.Where(l => l.Codigo.Contains(search) || l.Titulo.Contains(search));
        }
        licitaciones = query.Sort?.ToLowerInvariant() switch
        {
            "close_desc" => licitaciones.OrderByDescending(l => l.FechaCierreUtc),
            "code_desc" => licitaciones.OrderByDescending(l => l.Codigo),
            _ => licitaciones.OrderBy(l => l.Codigo)
        };
        var totalItems = await licitaciones.CountAsync(cancellationToken);
        var items = await licitaciones.Skip((query.ValidPage - 1) * query.ValidPageSize).Take(query.ValidPageSize).ToListAsync(cancellationToken);
        return new LicitacionPage(items.Select(l => LicitacionResponse.FromDomain(l, utcNow)).ToList(), totalItems, query.ValidPage, query.ValidPageSize);
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try { await _context.SaveChangesAsync(cancellationToken); }
        catch (DbUpdateConcurrencyException) { throw new LicitacionConcurrencyException(); }
    }
}
