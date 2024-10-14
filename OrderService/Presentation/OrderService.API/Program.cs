using Microsoft.AspNetCore.Mvc;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Infrastructure;
using OrderService.Core.Application.Features.Commands.Create;
using MediatR;
using OrderService.Core.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("CreateOrder", async ([FromBody] CreateOrderCommandRequest createOrderVM, IMediator mediator) =>
{
    await mediator.Send(createOrderVM);
});

app.Run();

