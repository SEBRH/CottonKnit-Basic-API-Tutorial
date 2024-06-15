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