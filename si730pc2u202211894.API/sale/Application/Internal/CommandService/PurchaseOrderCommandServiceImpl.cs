using si730pc2u202211894.API.sale.Domain.Models.Aggregates;
using si730pc2u202211894.API.sale.Domain.Models.Commands;
using si730pc2u202211894.API.sale.Domain.Models.ValueObjects;
using si730pc2u202211894.API.sale.Domain.Repositories;
using si730pc2u202211894.API.sale.Domain.Services;
using si730pc2u202211894.API.Shared.Domain.Repositories;

namespace si730pc2u202211894.API.sale.Application.Internal.CommandService;

public class PurchaseOrderCommandServiceImpl(IPurchaseOrderRepository purchaseOrderRepository, IUnitOfWork unitOfWork): IPurchaseOrderCommandService
{

    public async Task<PurchaseOrder> Handle(CreatePurchaseOrderCommand command)
    {
        bool existsByCustomerAndFabricId 
            = await purchaseOrderRepository
                .ExistsByCustomerAndFabricId
                    (command.Customer, command.FabricId);
        if (existsByCustomerAndFabricId)
        {
            throw new Exception($"Purchase order already exists for customer {command.Customer} and fabric {command.FabricId}");
        }

        if (!Enum.IsDefined(typeof(EFabricType), command.FabricId))
        {
            throw new Exception("Invalid Fabric Type");
        }
        
        var purchaseOrder = new PurchaseOrder(command);
        await purchaseOrderRepository.AddAsync(purchaseOrder);
        await unitOfWork.CompleteAsync();
        return purchaseOrder;

    }
}
