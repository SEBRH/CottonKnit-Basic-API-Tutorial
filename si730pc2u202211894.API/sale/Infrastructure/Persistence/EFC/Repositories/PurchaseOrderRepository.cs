using Microsoft.EntityFrameworkCore;
using si730pc2u202211894.API.sale.Domain.Models.Aggregates;
using si730pc2u202211894.API.sale.Domain.Models.ValueObjects;
using si730pc2u202211894.API.sale.Domain.Repositories;
using si730pc2u202211894.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using si730pc2u202211894.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace si730pc2u202211894.API.sale.Infrastructure.Persistence.EFC.Repositories;

public class PurchaseOrderRepository(AppDbContext context): BaseRepository<PurchaseOrder>(context), IPurchaseOrderRepository 
{
    public async Task<bool> ExistsByCustomerAndFabricId(string customer, int fabricId)
    {
        return await context.Set<PurchaseOrder>().AnyAsync(x => x.Customer == customer && x.FabricId == (EFabricType)fabricId);
    }
}