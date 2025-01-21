using MenuExtensions.Abstractions.Services;
using MenuExtensions.Models.VismaNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuExtensions.Controllers;

[ApiController]
[Route("[controller]")]
public class VismaNetController(IVismaNetService vismaNetService) : ControllerBase
{
    private readonly IVismaNetService _vismaNetService = vismaNetService;

    /// <summary>
    /// Gets all supplier classes.
    /// </summary>
    /// <returns>OkObjectResult</returns>
    [HttpGet]
    [Route("supplierClass")]
    [Authorize]
    public async Task<IActionResult> GetSupplierClasses()
    {
        // TODO: Add error handling
        var supplierClasses = await _vismaNetService.GetSupplierClasses();
        return Ok(supplierClasses);
    }

    /// <summary>
    /// Creates a supplier invoice.
    /// </summary>
    /// <param name="supplierInvoiceDto"></param>
    /// <returns>OkObjectResult</returns>
    [HttpPost]
    [Route("supplierInvoice")]
    [Authorize]
    public async Task<IActionResult> PostSupplierInvoice(
        [FromBody] CreateSupplierInvoiceDto supplierInvoiceDto
    )
    {
        // TODO: Add error handling
        await _vismaNetService.CreateSupplierInvoice(supplierInvoiceDto);
        return Ok("Supplier invoice created successfully");
    }
}
