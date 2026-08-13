using Licitaciones.Domain.Licitaciones;
using Licitaciones.Domain.Ofertas;
using Licitaciones.Domain.Proveedores;
using Licitaciones.Infrastructure.Persistence.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Licitaciones.Infrastructure.Persistence.Configurations;

public sealed class OfertaConfiguration : IEntityTypeConfiguration<Oferta>
{
    public void Configure(EntityTypeBuilder<Oferta> builder)
    {
        builder.ToTable("Ofertas", table =>
            table.HasCheckConstraint("CK_Ofertas_MontoPositivo", "\"MontoOfertadoCrc\" > 0"));
        builder.HasKey(oferta => oferta.Id);
        builder.Property(oferta => oferta.Id).ValueGeneratedNever();
        builder.Property(oferta => oferta.MontoOfertadoCrc).HasMoneyPrecision().IsRequired();
        builder.Property(oferta => oferta.FechaRegistro).HasColumnType("timestamp with time zone").IsRequired();
        builder.Property(oferta => oferta.UpdatedAt).IsRequired();
        builder.Property(oferta => oferta.Version).IsRowVersion();
        builder.HasOne<Licitacion>().WithMany().HasForeignKey(oferta => oferta.LicitacionId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Proveedor>().WithMany().HasForeignKey(oferta => oferta.ProveedorId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(oferta => new { oferta.LicitacionId, oferta.ProveedorId })
            .IsUnique()
            .HasDatabaseName("IX_Ofertas_LicitacionId_ProveedorId");
        builder.HasIndex(oferta => oferta.ProveedorId).HasDatabaseName("IX_Ofertas_ProveedorId");
    }
}
