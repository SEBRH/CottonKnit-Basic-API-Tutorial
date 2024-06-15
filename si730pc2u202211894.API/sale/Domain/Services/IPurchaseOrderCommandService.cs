using si730pc2u202211894.API.sale.Domain.Models.Aggregates;
using si730pc2u202211894.API.sale.Domain.Models.Commands;

namespace si730pc2u202211894.API.sale.Domain.Services;

public interface IPurchaseOrderCommandService
{
    Task<PurchaseOrder> Handle(CreatePurchaseOrderCommand command);
}