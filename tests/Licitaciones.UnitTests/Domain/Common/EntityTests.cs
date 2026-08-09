using Licitaciones.Domain.Common;

namespace Licitaciones.UnitTests.Domain.Common;

public sealed class EntityTests
{
    [Fact]
    public void EntitiesOfSameTypeWithSameIdAreEqual()
    {
        var id = Guid.NewGuid();

        var first = new TestEntity(id);
        var second = new TestEntity(id);

        Assert.Equal(first, second);
        Assert.True(first == second);
    }

    [Fact]
    public void EntityRejectsDefaultIdentifier()
    {
        var exception = Assert.Throws<DomainException>(() => new TestEntity(Guid.Empty));

        Assert.Contains("identifiers", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    private sealed class TestEntity(Guid id) : Entity<Guid>(id);
}
