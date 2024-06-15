using si730pc2u202211894.API.sale.Domain.Models.Commands;
using si730pc2u202211894.API.sale.Interfaces.REST.Resources;

namespace si730pc2u202211894.API.sale.Interfaces.REST.Transforms;

public class CreatePurchaseOrderCommandFromResourceAssembler
{
    public static CreatePurchaseOrderCommand ToCommandFromResource(CreatePurchaseOrderResource resource)
    {
        return new CreatePurchaseOrderCommand(resource.Customer, resource.FabricId, resource.City, resource.ResumeUrl, resource.Quantity);
    }
}