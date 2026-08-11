using Licitaciones.Application.Proveedores;
using Licitaciones.Web.Models.Proveedores;
using Microsoft.AspNetCore.Mvc;

namespace Licitaciones.Web.Controllers;

public sealed class ProveedoresController : Controller
{
    private readonly IProveedorService _service;

    public ProveedoresController(IProveedorService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(
        int page = 1,
        int pageSize = 10,
        string? search = null,
        string sort = "name",
        CancellationToken cancellationToken = default)
    {
        var result = await _service.ListAsync(new ProveedorQuery(page, pageSize, search, sort), cancellationToken);
        var pageResult = result.Value!;

        return View(new ProveedorIndexViewModel(
            pageResult.Items,
            pageResult.Page,
            pageResult.PageSize,
            pageResult.TotalItems,
            pageResult.TotalPages,
            search,
            sort));
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);

        if (!result.Succeeded)
        {
            return NotFound();
        }

        return View(result.Value);
    }

    public IActionResult Create()
    {
        return View(new ProveedorFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProveedorFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _service.CreateAsync(new CrearProveedorRequest(model.Nombre), cancellationToken);

        if (!result.Succeeded)
        {
            AddModelError(result);
            return View(model);
        }

        TempData["SuccessMessage"] = "Proveedor creado correctamente.";
        return RedirectToAction(nameof(Details), new { id = result.Value!.Id });
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);

        if (!result.Succeeded)
        {
            return NotFound();
        }

        ViewData["ProveedorId"] = id;
        return View(new ProveedorFormViewModel { Nombre = result.Value!.Nombre });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ProveedorFormViewModel model, CancellationToken cancellationToken)
    {
        ViewData["ProveedorId"] = id;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _service.UpdateAsync(id, new ActualizarProveedorRequest(model.Nombre), cancellationToken);

        if (!result.Succeeded)
        {
            if (result.Status == ProveedorResultStatus.NotFound)
            {
                return NotFound();
            }

            AddModelError(result);
            return View(model);
        }

        TempData["SuccessMessage"] = "Proveedor actualizado correctamente.";
        return RedirectToAction(nameof(Details), new { id = result.Value!.Id });
    }

    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);

        if (!result.Succeeded)
        {
            return NotFound();
        }

        return View(result.Value);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(id, cancellationToken);

        if (!result.Succeeded)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Proveedor retirado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private void AddModelError<T>(ProveedorResult<T> result)
    {
        ModelState.AddModelError(nameof(ProveedorFormViewModel.Nombre), result.ErrorMessage ?? "No fue posible guardar el proveedor.");
    }
}
