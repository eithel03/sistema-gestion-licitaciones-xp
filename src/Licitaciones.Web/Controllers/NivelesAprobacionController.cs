using Licitaciones.Application.Aprobaciones;
using Licitaciones.Web.Models.Aprobaciones;
using Microsoft.AspNetCore.Mvc;

namespace Licitaciones.Web.Controllers;

public sealed class NivelesAprobacionController : Controller
{
    private readonly INivelAprobacionService _service;

    public NivelesAprobacionController(INivelAprobacionService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var result = await _service.ListAsync(new NivelAprobacionQuery(page, pageSize), cancellationToken);
        return View(new NivelAprobacionIndexViewModel(result.Value!));
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return result.Succeeded ? View(result.Value) : NotFound();
    }

    public IActionResult Create() => View(new NivelAprobacionFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NivelAprobacionFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _service.CreateAsync(new CrearNivelAprobacionRequest(model.MontoMinimoCrc, model.MontoMaximoCrc, model.Aprobador), cancellationToken);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "No fue posible crear el nivel de aprobacion.");
            return View(model);
        }
        TempData["SuccessMessage"] = "Nivel de aprobacion creado correctamente.";
        return RedirectToAction(nameof(Details), new { id = result.Value!.Id });
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (!result.Succeeded) return NotFound();
        return View(new NivelAprobacionFormViewModel
        {
            MontoMinimoCrc = result.Value!.MontoMinimoCrc,
            MontoMaximoCrc = result.Value.MontoMaximoCrc,
            Aprobador = result.Value.Aprobador,
            Version = result.Value.Version
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, NivelAprobacionFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _service.UpdateAsync(id, new ActualizarNivelAprobacionRequest(model.MontoMinimoCrc, model.MontoMaximoCrc, model.Aprobador, model.Version), cancellationToken);
        if (!result.Succeeded)
        {
            if (result.Status == NivelAprobacionResultStatus.NotFound) return NotFound();
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "No fue posible editar el nivel de aprobacion.");
            return View(model);
        }
        TempData["SuccessMessage"] = "Nivel de aprobacion actualizado correctamente.";
        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return result.Succeeded ? View(result.Value) : NotFound();
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);
        if (!result.Succeeded)
        {
            if (result.Status == NivelAprobacionResultStatus.NotFound) return NotFound();
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Details), new { id });
        }
        TempData["SuccessMessage"] = "Nivel de aprobacion eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
