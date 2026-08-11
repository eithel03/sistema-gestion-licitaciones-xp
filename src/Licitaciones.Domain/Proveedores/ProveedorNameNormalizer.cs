using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Licitaciones.Domain.Proveedores;

public static partial class ProveedorNameNormalizer
{
    public static string NormalizeForDisplay(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return string.Empty;
        }

        return RepeatedSpacesRegex()
            .Replace(name.Trim().Normalize(NormalizationForm.FormC), " ");
    }

    public static string NormalizeForComparison(string? name)
    {
        var displayName = NormalizeForDisplay(name);

        return displayName
            .Normalize(NormalizationForm.FormKC)
            .ToUpper(CultureInfo.InvariantCulture);
    }

    public static bool HasAllowedCharacters(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        return AllowedCharactersRegex().IsMatch(name);
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex RepeatedSpacesRegex();

    [GeneratedRegex(@"^[\p{L}\p{N} .,\(\)]+$")]
    private static partial Regex AllowedCharactersRegex();
}
