using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Licitaciones.Domain.Licitaciones;

public static partial class LicitacionCodeNormalizer
{
    public static string NormalizeForDisplay(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return string.Empty;
        }

        return RepeatedSpacesRegex().Replace(code.Trim().Normalize(NormalizationForm.FormC), " ").ToUpper(CultureInfo.InvariantCulture);
    }

    public static string NormalizeForComparison(string? code)
    {
        return NormalizeForDisplay(code).Normalize(NormalizationForm.FormKC).ToUpper(CultureInfo.InvariantCulture);
    }

    public static bool HasAllowedCharacters(string code)
    {
        ArgumentNullException.ThrowIfNull(code);
        return AllowedCharactersRegex().IsMatch(code);
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex RepeatedSpacesRegex();

    [GeneratedRegex(@"^[\p{L}\p{N} -]+$")]
    private static partial Regex AllowedCharactersRegex();
}
