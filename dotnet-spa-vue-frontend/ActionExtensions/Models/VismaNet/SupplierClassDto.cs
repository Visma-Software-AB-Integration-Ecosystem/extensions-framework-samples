using static ActionExtensions.Models.VismaNet.SupplierClassDto.Attribute;

namespace ActionExtensions.Models.VismaNet;

/// <summary>
/// Represents a supplier class in Visma Net.
/// </summary>
/// <param name="Attributes"></param>
/// <param name="PaymentMethodId"></param>
/// <param name="PaymentMethodDescription"></param>
/// <param name="Id"></param>
/// <param name="Description"></param>
public record SupplierClassDto(
    IEnumerable<Attribute> Attributes,
    string PaymentMethodId,
    string PaymentMethodDescription,
    string Id,
    string Description
)
{
    public record Attribute(
        string AttributeId,
        string Description,
        int SortOrder,
        bool Required,
        string AttributeType,
        string DefaultValue,
        IEnumerable<Detail> Details
    )
    {
        public record Detail(string Id, string Description);
    }
}
