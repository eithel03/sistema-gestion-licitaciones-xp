using Licitaciones.Domain.Aprobaciones;
using Licitaciones.Infrastructure.Persistence.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Licitaciones.Infrastructure.Persistence.Configurations;

public sealed class NivelAprobacionConfiguration : IEntityTypeConfiguration<NivelAprobacion>
{
    public void Configure(EntityTypeBuilder<NivelAprobacion> builder)
    {
        builder.ToTable("NivelesAprobacion", table =>
        {
            table.HasCheckConstraint("CK_NivelesAprobacion_MinimoPositivo", "\"MontoMinimoCrc\" > 0");
            table.HasCheckConstraint("CK_NivelesAprobacion_MaximoValido", "\"MontoMaximoCrc\" IS NULL OR \"MontoMaximoCrc\" >= \"MontoMinimoCrc\"");
        });
        builder.HasKey(nivel => nivel.Id);
        builder.Property(nivel => nivel.Id).ValueGeneratedNever();
        builder.Property(nivel => nivel.MontoMinimoCrc).HasMoneyPrecision().IsRequired();
        builder.Property(nivel => nivel.MontoMaximoCrc).HasMoneyPrecision();
        builder.Property(nivel => nivel.Aprobador).HasMaxLength(NivelAprobacion.AprobadorMaxLength).IsRequired();
        builder.Property(nivel => nivel.Version).IsRowVersion();
        builder.Ignore(nivel => nivel.IsOpen);
        builder.HasIndex(nivel => nivel.MontoMinimoCrc).HasDatabaseName("IX_NivelesAprobacion_MontoMinimoCrc");
        builder.HasIndex(nivel => nivel.MontoMaximoCrc)
            .IsUnique()
            .AreNullsDistinct(false)
            .HasFilter("\"MontoMaximoCrc\" IS NULL")
            .HasDatabaseName("IX_NivelesAprobacion_UnicoRangoAbierto");
    }
}
