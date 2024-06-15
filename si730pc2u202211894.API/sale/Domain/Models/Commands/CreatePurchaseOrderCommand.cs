namespace si730pc2u202211894.API.sale.Domain.Models.Commands;

public record CreatePurchaseOrderCommand(
    string Customer,
    int FabricId,
    string City,
    string ResumeUrl,
    int Quantity
    
    );