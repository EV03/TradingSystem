using Domain.Enterprise.repository;
using Enterprise.Application;
using Enterprise.Application.Services;
using Enterprise.Integration;
using Microsoft.EntityFrameworkCore;
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







var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();
