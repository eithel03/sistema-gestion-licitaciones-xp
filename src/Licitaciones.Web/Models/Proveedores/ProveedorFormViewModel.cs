using System.ComponentModel.DataAnnotations;
using Licitaciones.Domain.Proveedores;

namespace Licitaciones.Web.Models.Proveedores;

public sealed class ProveedorFormViewModel : IValidatableObject
{
    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre del proveedor es requerido.")]
    [StringLength(200, ErrorMessage = "El nombre no debe superar 200 caracteres.")]
    public string? Nombre { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var normalized = ProveedorNameNormalizer.NormalizeForDisplay(Nombre);
        if (string.IsNullOrWhiteSpace(normalized))
        {
            if (!string.IsNullOrWhiteSpace(Nombre))
            {
                yield return new ValidationResult("El nombre del proveedor es requerido.", [nameof(Nombre)]);
            }

            yield break;
        }

        if (!ProveedorNameNormalizer.HasAllowedCharacters(normalized))
        {
            yield return new ValidationResult(
                "El nombre del proveedor solo puede contener letras, numeros, espacios, punto, coma y parentesis.",
                [nameof(Nombre)]);
        }
    }
}
