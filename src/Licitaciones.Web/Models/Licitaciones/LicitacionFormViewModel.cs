using System.ComponentModel.DataAnnotations;

namespace Licitaciones.Web.Models.Licitaciones;

public sealed class LicitacionFormViewModel
{
    [Display(Name = "Codigo")]
    [Required(ErrorMessage = "El codigo de la licitacion es requerido.")]
    [RegularExpression(
        @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ0-9 -]+$",
        ErrorMessage = "Use solo letras, numeros, espacios y guion.")]
    [StringLength(50, ErrorMessage = "El codigo no debe superar 50 caracteres.")]
    public string? Codigo { get; init; }

    [Display(Name = "Titulo")]
    [Required(ErrorMessage = "El titulo de la licitacion es requerido.")]
    [StringLength(200, ErrorMessage = "El titulo no debe superar 200 caracteres.")]
    public string? Titulo { get; init; }

    [Display(Name = "Presupuesto CRC")]
    [Range(0.01, 9999999999999999, ErrorMessage = "El presupuesto debe ser mayor que cero.")]
    public decimal PresupuestoCrc { get; init; }

    [Display(Name = "Fecha de cierre")]
    [Required(ErrorMessage = "La fecha de cierre es requerida.")]
    public DateTime FechaCierreLocal { get; init; }

    public uint? Version { get; init; }
}
