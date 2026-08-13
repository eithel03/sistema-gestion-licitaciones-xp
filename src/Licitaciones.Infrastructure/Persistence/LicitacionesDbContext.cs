using Licitaciones.Domain.Licitaciones;
using Licitaciones.Domain.Proveedores;
using Licitaciones.Domain.Ofertas;
using Licitaciones.Domain.Aprobaciones;
using Licitaciones.Infrastructure.Persistence.Conventions;
using Microsoft.EntityFrameworkCore;

namespace Licitaciones.Infrastructure.Persistence;

public sealed class LicitacionesDbContext : DbContext
{
    public LicitacionesDbContext(DbContextOptions<LicitacionesDbContext> options)
        : base(options)
    {
    }

    public DbSet<Proveedor> Proveedores => Set<Proveedor>();

    public DbSet<Licitacion> Licitaciones => Set<Licitacion>();

    public DbSet<Oferta> Ofertas => Set<Oferta>();

    public DbSet<NivelAprobacion> NivelesAprobacion => Set<NivelAprobacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LicitacionesDbContext).Assembly);
        modelBuilder.ApplyPersistenceConventions();
    }
}
