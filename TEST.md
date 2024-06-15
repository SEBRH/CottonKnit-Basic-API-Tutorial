# Creating this kind of Projects

## Loading the Shared Services and the Project Configurations

Be sure that you are creating the project as

- Web
  - Web Api Following what the test asks you to create

### Using the Shared

In these cases when we implement the shared we MUST change the namespace to the current namespace we will be using.

Like in these example:
```csharp
namespace si730pc2u202211894.API.Shared.Infrastructure.Persistence.EFC.Repositories;
```

Keep in mind that we also should change the namespace of the `AppDbContext` and the `Entity` to the current namespace we are using.
And all the other classes that are in the shared.

`REMEMBER` the changes to the `AppDBContext` will be made once you have finished 
the solution since then you will have the Aggregates and such for the AppDbContext.

### Using the Configurations

`appsettings.json` use database = to whatever the schema is called
```csharp

{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost,3306;user=root;password=password;database=cottonknit;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

```


`Startup.cs` use the following code to configure the database and REMEMBER TO ADD THE SERVICES

COMMAND SERVICE AND REPOSITORIES

ALSO, DON`T FORGET TO MAKE THE SWAGGER DOCUMENTATION

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using si730pc2u202211894.API.sale.Application.Internal.CommandService;
using si730pc2u202211894.API.sale.Domain.Repositories;
using si730pc2u202211894.API.sale.Domain.Services;
using si730pc2u202211894.API.sale.Infrastructure.Persistence.EFC.Repositories;
using si730pc2u202211894.API.Shared.Domain.Repositories;
using si730pc2u202211894.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using si730pc2u202211894.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using si730pc2u202211894.API.Shared.Interfaces.ASP.Configuration;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.
builder.Services.AddControllers( options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));


//Add Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Configure Database Context and Logging Levels

builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        if (connectionString != null)
            if (builder.Environment.IsDevelopment())
                options.UseMySQL(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            else if (builder.Environment.IsProduction())
                options.UseMySQL(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Error)
                    .EnableDetailedErrors();    
    });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "Cotton_Knit.Platform.API",
                Version = "v1",
                Description = "Cotton_Knit Platform API",
                TermsOfService = new Uri("https://cotton_knit.com/tos"),
                Contact = new OpenApiContact
                {
                    Name = "Cotton_Knit Studios",
                    Email = "contact@CK.com"
                },
                License = new OpenApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
                }
            });
        c.EnableAnnotations();
    });




// Configure Lowercase URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Configure Dependency Injection

// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//hr Bounded Context Injection Configuration
builder.Services.AddScoped<IPurchaseOrderCommandService, PurchaseOrderCommandServiceImpl>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();




var app = builder.Build();

// Verify Database Objects are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

### Configuration for Dependencies

Do not forget this will be found in the `csproj` file under the File System View.

```csharp
<Project Sdk="Microsoft.NET.Sdk.Web">

    <PropertyGroup>
        <TargetFramework>net8.0</TargetFramework>
        <Nullable>enable</Nullable>
        <ImplicitUsings>enable</ImplicitUsings>
    </PropertyGroup>

    <ItemGroup>
        <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="8.0.6"/>
        <PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0"/>
        <PackageReference Include="EntityFrameworkCore.CreatedUpdatedDate" Version="8.0.0" />
        <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.4" />
        <PackageReference Include="Microsoft.EntityFrameworkCore.Relational" Version="8.0.4" />
        <PackageReference Include="Microsoft.EntityFrameworkCore.Relational.Design" Version="1.1.6" />
        <PackageReference Include="Humanizer" Version="2.14.1" />
        <PackageReference Include="MySql.EntityFrameworkCore" Version="8.0.2" />
        <PackageReference Include="Swashbuckle.AspNetCore.Annotations" Version="6.5.0"/> </ItemGroup>

</Project>

```

## Bounded Context Domain/Models

### Aggregates

```csharp
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

```

#### Audit Creation and Update Date

```csharp

using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.CreatedUpdatedDate.Contracts;

namespace si730pc2u202211894.API.sale.Domain.Models.Aggregates;

public partial class PurchaseOrder : IEntityWithCreatedUpdatedDate
{
    [Column("CreatedAt")] public DateTimeOffset? CreatedDate { get; set; }
    [Column("UpdatedAt")] public DateTimeOffset? UpdatedDate { get; set; }
}

```
### Value Objects

```csharp

namespace si730pc2u202211894.API.sale.Domain.Models.ValueObjects;

public enum EFabricType
{
    Algodon,
    Modal,
    Elastano,
    Poliester,
    Nailon,
    Acrilico,
    Rayon,
    Lyocell
}

```

### Commands

```csharp
namespace si730pc2u202211894.API.sale.Domain.Models.Commands;

public record CreatePurchaseOrderCommand(
    string Customer,
    int FabricId,
    string City,
    string ResumeUrl,
    int Quantity
    );

```

## Domain/Repositories

### Repository Interface

```csharp
using si730pc2u202211894.API.sale.Domain.Models.Aggregates;
using si730pc2u202211894.API.Shared.Domain.Repositories;

namespace si730pc2u202211894.API.sale.Domain.Repositories;

public interface IPurchaseOrderRepository : IBaseRepository<PurchaseOrder>
{
    Task<bool> ExistsByCustomerAndFabricId(string customer, int fabricId);
}
```

### Command Service Interface

```csharp
using si730pc2u202211894.API.sale.Domain.Models.Aggregates;
using si730pc2u202211894.API.sale.Domain.Models.Commands;

namespace si730pc2u202211894.API.sale.Domain.Services;

public interface IPurchaseOrderCommandService
{
    Task<PurchaseOrder> Handle(CreatePurchaseOrderCommand command);
}
```

## Bounded Context Infrastructure/Persistence/EFC/Repositories

### Repository Implementation

```csharp
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
```
## Bounded Context Application/Internal/CommandService

### Command Service Implementation

```csharp
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
```
## Bounded Context Interfaces/REST

### Resources

#### Create Resource

```csharp
public record CreatePurchaseOrderResource(string Customer, int FabricId, string City, string ResumeUrl, int Quantity );
```

#### Resource 

Remember that this is what will be shown after the creation of the object

```csharp

namespace si730pc2u202211894.API.sale.Interfaces.REST.Resources;

public record PurchaseOrderResource(int Id,string Customer, string FabricType, string City, string ResumeUrl, int Quantity );

```

### Transforms

#### Create_CommandFromResourceAssembler
```csharp
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
```

#### _ResourceFromEntityAssembler

```csharp
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
```

### Controllers

```csharp
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
```

### Final Observations

Don't Forget that `AppDbContext` is to be changed along with the `Program.cs` once you already have made all the configurations relating to the Bounded Context.