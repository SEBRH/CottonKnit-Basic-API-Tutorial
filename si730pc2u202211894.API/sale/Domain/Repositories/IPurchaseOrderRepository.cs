using si730pc2u202211894.API.sale.Domain.Models.Aggregates;
using si730pc2u202211894.API.Shared.Domain.Repositories;

namespace si730pc2u202211894.API.sale.Domain.Repositories;

public interface IPurchaseOrderRepository : IBaseRepository<PurchaseOrder>
{
    Task<bool> ExistsByCustomerAndFabricId(string customer, int fabricId);
}