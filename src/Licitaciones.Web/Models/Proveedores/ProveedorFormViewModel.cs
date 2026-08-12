using System.ComponentModel.DataAnnotations;

namespace Licitaciones.Web.Models.Proveedores;

public sealed class ProveedorFormViewModel
{
    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El nombre del proveedor es requerido.")]
    [RegularExpression(@"^[\p{L}\p{N} .,()]+$", ErrorMessage = "Use solo letras, numeros, espacios, punto, coma y parentesis.")]
    [StringLength(200, ErrorMessage = "El nombre no debe superar 200 caracteres.")]
    public string? Nombre { get; init; }
}
