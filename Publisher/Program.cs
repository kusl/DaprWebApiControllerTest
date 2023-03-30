using Dapr.Client;
using Man.Dapr.Sidekick;
using Microsoft.AspNetCore.Mvc;
using Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddDapr();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDaprSidekick();
builder.Services.AddDaprClient();

builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    _ = configuration.ReadFrom.Configuration(hostContext.Configuration);
});

builder.Services.AddRouting();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting();
app.UseCloudEvents();

app.MapGet("/status1", ([FromServices] IDaprSidecarHost daprSidecarHost) =>
{
    return Results.Ok(new
    {
        process = daprSidecarHost.GetProcessInfo(),
        options = daprSidecarHost.GetProcessOptions()
    });
});

app.MapPost("/publish1", async (MyOrder order) =>
{
    using var client = new DaprClientBuilder().Build();
    await client.PublishEventAsync("pubsub", "myorders", order);
    Console.WriteLine($"Published order with order id {order.MyOrderId} and name {order.MyOrderName}");
    return Results.Ok(order);
});


await app.RunAsync();
