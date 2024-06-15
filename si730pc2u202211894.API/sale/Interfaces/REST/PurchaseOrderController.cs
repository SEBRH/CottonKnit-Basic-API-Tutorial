using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using si730pc2u202211894.API.sale.Domain.Services;
using si730pc2u202211894.API.sale.Interfaces.REST.Resources;
using si730pc2u202211894.API.sale.Interfaces.REST.Transforms;

namespace si730pc2u202211894.API.sale.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class PurchaseOrderController(IPurchaseOrderCommandService purchaseOrderCommandService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseOrder(CreatePurchaseOrderResource resource)
    {
        var createPurchaseOrderCommand = CreatePurchaseOrderCommandFromResourceAssembler.ToCommandFromResource(resource);
        var purchaseOrder = await purchaseOrderCommandService.Handle(createPurchaseOrderCommand);
        var purchaseOrderResource = PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity(purchaseOrder);
        return StatusCode(201, purchaseOrderResource);
    }
    
}
