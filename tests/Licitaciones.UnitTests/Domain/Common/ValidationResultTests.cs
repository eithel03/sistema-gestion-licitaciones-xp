using Licitaciones.Domain.Common;

namespace Licitaciones.UnitTests.Domain.Common;

public sealed class ValidationResultTests
{
    [Fact]
    public void SuccessResultHasNoErrors()
    {
        var result = ValidationResult.Success();

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void FailureResultKeepsValidationErrors()
    {
        var error = new ValidationError("supplier.name.required", "Supplier name is required.");

        var result = ValidationResult.Failure(error);

        Assert.False(result.IsValid);
        Assert.Contains(error, result.Errors);
    }

    [Fact]
    public void FailureRequiresAtLeastOneError()
    {
        var exception = Assert.Throws<ArgumentException>(() => ValidationResult.Failure());

        Assert.Equal("errors", exception.ParamName);
    }
}
