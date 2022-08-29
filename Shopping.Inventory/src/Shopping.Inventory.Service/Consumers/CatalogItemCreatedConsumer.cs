using MassTransit;
using Shopping.Catalog.Contracts;
using Shopping.Inventory.Service.Models;
using Shopping.Common;

namespace Shopping.Inventory.Service.Consumers;

public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
{
    private readonly IRepository<CatalogItem> repository;

    public CatalogItemCreatedConsumer(IRepository<CatalogItem> repository)
    {
        this.repository = repository;
    }

    public async Task Consume(ConsumeContext<CatalogItemCreated> context)
    {
        var message = context.Message;

        var item = await repository.GetAsync(message.ItemId);

        if(item != null)
            return;
        
        item = new CatalogItem{
            Id = message.ItemId,
            Name = message.Name,
            Description = message.Description
        };

        await repository.CreateAsync(item);
    }
}