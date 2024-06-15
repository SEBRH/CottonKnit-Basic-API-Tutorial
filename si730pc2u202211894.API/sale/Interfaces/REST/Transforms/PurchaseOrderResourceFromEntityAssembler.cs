using si730pc2u202211894.API.sale.Domain.Models.Aggregates;
using si730pc2u202211894.API.sale.Interfaces.REST.Resources;

namespace si730pc2u202211894.API.sale.Interfaces.REST.Transforms;

public class PurchaseOrderResourceFromEntityAssembler
{
    public static PurchaseOrderResource ToResourceFromEntity(PurchaseOrder entity)
        {
            return new PurchaseOrderResource(entity.Id, entity.Customer, entity.FabricType, entity.City, entity.ResumeUrl, entity.Quantity);
        }
}