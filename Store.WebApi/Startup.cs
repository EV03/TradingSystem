﻿using Microsoft.EntityFrameworkCore;
using Domain.StoreSystem;
using Domain.StoreSystem.repository;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Shared.Contracts.Events;
using Shared.Contracts.Mapping;
using Store.Application;
using Store.Application.EventHandlers;
using Store.Application.service;
using Store.Integration;
using ProductService = Store.Grpc.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Kestrel configuration
builder.WebHost.ConfigureKestrel(options =>
{
    var urls = builder.Configuration["ASPNETCORE_URLS"]?.Split(';');
    if (urls != null)
    {
        foreach (var url in urls)
        {
            var uri = new Uri(url);
            options.ListenAnyIP(uri.Port, listenOptions =>
            {
                if (uri.Scheme == Uri.UriSchemeHttps)
                {
                    listenOptions.UseHttps();
                    listenOptions.Protocols = HttpProtocols.Http2; // Only HTTP/2 on HTTPS
                }
                else
                {
                    listenOptions.Protocols = HttpProtocols.Http1;
                }
            });
        }
    }
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
builder.Services.AddScoped<IEventHandler<InventorySyncReqEvent>, InventorySyncReqEventHandler>();
builder.Services.AddScoped<IEventHandler<ProductTransferEvent>, ProductTransferEventHandler>();


builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
             builder
                .SetIsOriginAllowed(origin => 
                    origin.StartsWith("http://localhost:") || 
                    origin.StartsWith("http://127.0.0.1:") ||
                    origin.StartsWith("https://localhost:") || 
                    origin.StartsWith("https://127.0.0.1:"))
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
}
// subscribe to events
var eventBus = app.Services.GetRequiredService<IEventBus>();
await eventBus.SubscribeAsync<InventorySyncReqEvent, InventorySyncReqEventHandler>();
await eventBus.SubscribeAsync<ProductTransferEvent, ProductTransferEventHandler>();
app.Run();