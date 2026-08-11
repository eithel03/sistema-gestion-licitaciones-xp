using Microsoft.AspNetCore.Mvc;

namespace Licitaciones.Web.Controllers;

public sealed class PlanificadosController : Controller
{
    public IActionResult Licitaciones()
    {
        ViewData["Modulo"] = "Licitaciones";
        return View("Modulo");
    }

    public IActionResult Ofertas()
    {
        ViewData["Modulo"] = "Ofertas";
        return View("Modulo");
    }

    public IActionResult NivelesAprobacion()
    {
        ViewData["Modulo"] = "Niveles de aprobacion";
        return View("Modulo");
    }

    public IActionResult TipoCambio()
    {
        ViewData["Modulo"] = "Tipo de cambio";
        return View("Modulo");
    }

    public IActionResult ApiSwagger()
    {
        return View();
    }
}
