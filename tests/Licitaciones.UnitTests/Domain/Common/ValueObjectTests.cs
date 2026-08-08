using Licitaciones.Domain.Common;

namespace Licitaciones.UnitTests.Domain.Common;

public sealed class ValueObjectTests
{
    [Fact]
    public void ValueObjectsOfSameTypeWithSameComponentsAreEqual()
    {
        var first = new TestValueObject("ABC", 10);
        var second = new TestValueObject("ABC", 10);

        Assert.Equal(first, second);
        Assert.True(first == second);
    }

    [Fact]
    public void ValueObjectsWithDifferentComponentsAreNotEqual()
    {
        var first = new TestValueObject("ABC", 10);
        var second = new TestValueObject("ABC", 20);

        Assert.NotEqual(first, second);
        Assert.True(first != second);
    }

    private sealed class TestValueObject(string code, int sequence) : ValueObject
    {
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return code;
            yield return sequence;
        }
    }
}
