using ActionExtensions.Models.VismaNet;

namespace ActionExtensions.Abstractions.Services;

public interface IVismaNetService
{
    /// <summary>
    /// Gets all suppliers classes.
    /// </summary>
    /// <returns>Task<IEnumerable<SupplierClassDto>></returns>
    Task<IEnumerable<SupplierClassDto>> GetSupplierClasses();

    /// <summary>
    /// Creates a supplier invoice.
    /// </summary>
    /// <param name="supplierInvoiceDto"></param>
    /// <returns>Task</returns>
    Task CreateSupplierInvoice(CreateSupplierInvoiceDto supplierInvoiceDto);
}
