
using Licitaciones.Domain.Licitaciones;
using Licitaciones.Infrastructure.Persistence.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Licitaciones.Infrastructure.Persistence.Configurations;

public sealed class LicitacionConfiguration : IEntityTypeConfiguration<Licitacion>
{
    public void Configure(EntityTypeBuilder<Licitacion> builder)
    {
        builder.ToTable("Licitaciones");
        builder.HasKey(licitacion => licitacion.Id);
        builder.Property(licitacion => licitacion.Id).ValueGeneratedNever();
        builder.Property(licitacion => licitacion.Codigo).HasMaxLength(Licitacion.CodigoMaxLength).IsRequired();
        builder.Property(licitacion => licitacion.CodigoNormalizado).HasMaxLength(Licitacion.CodigoMaxLength).IsRequired();
        builder.Property(licitacion => licitacion.Titulo).HasMaxLength(Licitacion.TituloMaxLength).IsRequired();
        builder.Property(licitacion => licitacion.PresupuestoCrc).HasMoneyPrecision().IsRequired();
        builder.Property(licitacion => licitacion.FechaCierreUtc).IsRequired();
        builder.Property(licitacion => licitacion.Estado).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(licitacion => licitacion.Version).IsRowVersion();
        builder.HasIndex(licitacion => licitacion.CodigoNormalizado).IsUnique().HasFilter("\"DeletedAt\" IS NULL").HasDatabaseName("IX_Licitaciones_CodigoNormalizado");
        builder.HasIndex(licitacion => licitacion.Estado).HasDatabaseName("IX_Licitaciones_Estado");
        builder.HasQueryFilter(licitacion => licitacion.DeletedAt == null);
    }
}
