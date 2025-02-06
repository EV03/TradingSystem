using Domain.Enterprise.repository;
using Enterprise.Application;
using Enterprise.Application.Services;
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

builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();
builder.Services.AddTransient<IEventHandler<LowStockEvent>, LowStockEventHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
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
    
    // subscribe to events
    var eventBus = serviceScope.ServiceProvider.GetRequiredService<IEventBus>();
    await eventBus.SubscribeAsync<LowStockEvent, LowStockEventHandler>();
}
app.Run();
