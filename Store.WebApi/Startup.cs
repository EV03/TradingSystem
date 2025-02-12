using Microsoft.EntityFrameworkCore;
using Domain.StoreSystem;
using Domain.StoreSystem.repository;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Shared.Contracts.Events;
using Shared.Contracts.Mapping;
using Store.Application;
using Store.Application.service;
using Store.Integration;
using ProductService = Store.Grpc.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Kestrel für HTTP/2 konfigurieren
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5131, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });

    options.ListenAnyIP(7138, listenOptions =>
    {
        listenOptions.UseHttps();
        listenOptions.Protocols = HttpProtocols.Http2; // Nur HTTP/2 auf HTTPS
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseInMemoryDatabase("StoreSystem"));
builder.Services.AddGrpc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IStockItemRepository, StockItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, Store.Application.ProductService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IOrderService, OrderService>();


builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:*")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGrpcService<ProductService>();
});

app.MapGet("/", () => "Hello World");

// Ensure the database is created and seeded with initial data
using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<StoreContext>();
    context.Database.EnsureCreated();
    
    var eventBus = serviceScope.ServiceProvider.GetRequiredService<IEventBus>();
}

app.Run();