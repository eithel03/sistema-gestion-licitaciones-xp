using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Licitaciones.Infrastructure.Persistence.Conventions;

public static class PersistenceModelBuilderExtensions
{
    private const string TimestampWithTimeZone = "timestamp with time zone";

    public static ModelBuilder ApplyPersistenceConventions(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ConfigureUtcTimestamp(entityType.FindProperty(PersistencePropertyNames.CreatedAt));
            ConfigureUtcTimestamp(entityType.FindProperty(PersistencePropertyNames.UpdatedAt));
            ConfigureUtcTimestamp(entityType.FindProperty(PersistencePropertyNames.DeletedAt));
            ConfigureOptimisticConcurrency(entityType.FindProperty(PersistencePropertyNames.Version));
        }

        return modelBuilder;
    }

    private static void ConfigureUtcTimestamp(IMutableProperty? property)
    {
        if (property is null)
        {
            return;
        }

        if (property.ClrType == typeof(DateTimeOffset) || property.ClrType == typeof(DateTimeOffset?))
        {
            property.SetColumnType(TimestampWithTimeZone);
        }
    }

    private static void ConfigureOptimisticConcurrency(IMutableProperty? property)
    {
        if (property is null)
        {
            return;
        }

        property.IsConcurrencyToken = true;
    }
}
