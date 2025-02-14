using Domain.Enterprise.repository;
using Enterprise.Application;
using Enterprise.Application.Services;
using Enterprise.Application.EventHandlers;
using Enterprise.Integration;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Events;
using Shared.Contracts.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<EnterpriseContext>(options =>
    options.UseInMemoryDatabase("EnterpriseSystem"));

builder.Services.AddScoped<IDeliveryLogRepository, DeliveryLogLogRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IOrderProcessingService, OrderProcessingService>();

builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();
builder.Services.AddScoped<OrderCreatedEventHandler>();
builder.Services.AddScoped<IEventHandler<OrderCreatedEvent>, OrderCreatedEventHandler>();
builder.Services.AddScoped<IEventHandler<OrderDeliveredEvent>, OrderDeliveredEventHandler>();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowSpecificOrigin"); // Add this line to use the CORS policy
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapGet("/", () => "Hello World");

using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<EnterpriseContext>();
    context.Database.EnsureCreated();
}

// subscribe to events
var eventBus = app.Services.GetRequiredService<IEventBus>();
await eventBus.SubscribeAsync<OrderCreatedEvent, OrderCreatedEventHandler>();
await eventBus.SubscribeAsync<OrderDeliveredEvent, OrderDeliveredEventHandler>();
app.Run();