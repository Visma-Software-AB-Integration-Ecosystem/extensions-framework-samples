using static ActionExtensions.Models.VismaNet.CreateSupplierInvoiceDto;

namespace ActionExtensions.Models.VismaNet;

/// <summary>
/// Base values to be used for creating a supplier invoice in Visma Net.
/// To find all the possible values for the properties, see the Visma Net API documentation.
/// </summary>
/// <param name="SupplierNumber"></param>
/// <param name="SupplierReference"></param>
/// <param name="InvoiceLines"></param>
public record CreateSupplierInvoiceDto(
    ValueType<string> SupplierNumber,
    ValueType<string> SupplierReference,
    IEnumerable<InvoiceLine> InvoiceLines
)
{
    public DateTime Date => DateTime.Now;

    public record InvoiceLine(ValueType<int> LineNumber, ValueType<string> InventoryNumber)
    {
        // For creating a supplier invoice, this value should always be "insert"
        public string Operation => "insert";

        public record SubAccount(int SegmentId, string SegmentValue);
    }
}
