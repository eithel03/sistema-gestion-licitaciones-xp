using Licitaciones.Application.Proveedores;
using Licitaciones.Domain.Proveedores;
using Microsoft.EntityFrameworkCore;

namespace Licitaciones.Infrastructure.Persistence.Repositories;

public sealed class ProveedorRepository : IProveedorRepository
{
    private readonly LicitacionesDbContext _context;

    public ProveedorRepository(LicitacionesDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Proveedor proveedor, CancellationToken cancellationToken = default)
    {
        await _context.Proveedores.AddAsync(proveedor, cancellationToken);
    }

    public Task<Proveedor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _context.Proveedores
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(proveedor => proveedor.Id == id, cancellationToken);
    }

    public Task<bool> ExistsByNormalizedNameAsync(
        string normalizedName,
        Guid? excludedId = null,
        CancellationToken cancellationToken = default)
    {
        return _context.Proveedores.AnyAsync(
            proveedor => proveedor.NombreNormalizado == normalizedName
                && (!excludedId.HasValue || proveedor.Id != excludedId.Value),
            cancellationToken);
    }

    public async Task<ProveedorPage> ListAsync(ProveedorQuery query, CancellationToken cancellationToken = default)
    {
        var proveedores = _context.Proveedores.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var normalizedSearch = query.Search.Trim();
            proveedores = proveedores.Where(proveedor => EF.Functions.ILike(proveedor.Nombre, $"%{normalizedSearch}%"));
        }

        proveedores = string.Equals(query.Sort, "name_desc", StringComparison.OrdinalIgnoreCase)
            ? proveedores.OrderByDescending(proveedor => proveedor.Nombre)
            : proveedores.OrderBy(proveedor => proveedor.Nombre);

        var totalItems = await proveedores.CountAsync(cancellationToken);
        var items = await proveedores
            .Skip((query.ValidPage - 1) * query.ValidPageSize)
            .Take(query.ValidPageSize)
            .Select(proveedor => new ProveedorResponse(
                proveedor.Id,
                proveedor.Nombre,
                proveedor.NombreNormalizado,
                proveedor.CreatedAt,
                proveedor.UpdatedAt,
                proveedor.DeletedAt))
            .ToListAsync(cancellationToken);

        return new ProveedorPage(items, totalItems, query.ValidPage, query.ValidPageSize);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
