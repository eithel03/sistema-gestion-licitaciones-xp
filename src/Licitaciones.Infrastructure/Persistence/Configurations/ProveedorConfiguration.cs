using Licitaciones.Domain.Proveedores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Licitaciones.Infrastructure.Persistence.Configurations;

public sealed class ProveedorConfiguration : IEntityTypeConfiguration<Proveedor>
{
    public void Configure(EntityTypeBuilder<Proveedor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("Proveedores");

        builder.HasKey(proveedor => proveedor.Id);

        builder.Property(proveedor => proveedor.Id)
            .ValueGeneratedNever();

        builder.Property(proveedor => proveedor.Nombre)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(proveedor => proveedor.NombreNormalizado)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(proveedor => proveedor.NombreNormalizado)
            .IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL")
            .HasDatabaseName("IX_Proveedores_NombreNormalizado");

        builder.HasQueryFilter(proveedor => proveedor.DeletedAt == null);
    }
}
