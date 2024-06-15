using System.ComponentModel.DataAnnotations;
using si730pc2u202211894.API.sale.Domain.Models.Commands;
using si730pc2u202211894.API.sale.Domain.Models.ValueObjects;

namespace si730pc2u202211894.API.sale.Domain.Models.Aggregates;

public partial class PurchaseOrder
{
    public int Id { get; set; }
    
    
    
    [Required]
    public string Customer { get; set; }
    [Required]
    public EFabricType FabricId { get; set; }
    
    [Required]
    public string City { get; set; }
    
    [Required]
    public string ResumeUrl { get; set; }
    
    [Required]
    public int Quantity { get; set; }
    
    public string FabricType => FabricId.ToString();
    
    public PurchaseOrder(){}
    
    public PurchaseOrder(CreatePurchaseOrderCommand command)
    {
        this.Customer = command.Customer;
        this.FabricId = (EFabricType)command.FabricId;
        this.City = command.City;
        this.ResumeUrl = command.ResumeUrl;
        this.Quantity = command.Quantity;
    }
    
}
