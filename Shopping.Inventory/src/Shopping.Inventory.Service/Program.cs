using Polly;
using Polly.Timeout;
using Shopping.Common.MongoDB;
using Shopping.Inventory.Service.Models;
using Shopping.Inventory.Service.Clients;
using Shopping.Common.MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMongo()
                .AddMongoRepository<InventoryItem>("inventoryitems")
                .AddMongoRepository<CatalogItem>("catalogitems")
                .AddMassTransitWithRabbitMq();

Random jitterer = new Random();

builder.Services.AddHttpClient<CatalogClient>(client => 
{
    client.BaseAddress = new Uri("https://localhost:7085");
})
.AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(
    5,
    retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)) 
                    + TimeSpan.FromMilliseconds(jitterer.Next(0,1000))
))
.AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
    3,
    TimeSpan.FromSeconds(15)
))
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
