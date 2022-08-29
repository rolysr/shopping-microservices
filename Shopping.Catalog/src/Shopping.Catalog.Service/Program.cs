using Shopping.Catalog.Service.Models;
using Shopping.Common.Settings;
using Shopping.Common.MongoDB;
using Shopping.Common.MassTransit;
using MassTransit;

ServiceSettings serviceSettings;
var builder = WebApplication.CreateBuilder(args);

//Getting the services settings from appsettings.json
serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

builder.Services.AddControllers(options => 
{
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMongo().AddMongoRepository<Item>("item").AddMassTransitWithRabbitMq();

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
