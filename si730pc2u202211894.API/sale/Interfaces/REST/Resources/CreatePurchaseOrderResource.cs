﻿namespace si730pc2u202211894.API.sale.Interfaces.REST.Resources;

public record CreatePurchaseOrderResource(string Customer, int FabricId, string City, string ResumeUrl, int Quantity );