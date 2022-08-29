using Microsoft.AspNetCore.Mvc;
using Shopping.Catalog.Service.Models;
using Shopping.Catalog.Service.Dtos;
using Shopping.Common;
using Shopping.Catalog.Contracts;
using MassTransit;

namespace Shopping.Catalog.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private readonly IRepository<Item> itemsRepository;
    private readonly IPublishEndpoint publishEndpoint;

    public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
    {
        this.itemsRepository = itemsRepository;
        this.publishEndpoint = publishEndpoint;
    }

    // GET /items
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
    {
        var items = (await itemsRepository.GetAllAsync())
                    .Select(item => item.AsDto());
        
        return Ok(items);
    }

    // GET /items/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
    {
        var item = await itemsRepository.GetAsync(id);       
        if(item is null)
            return NotFound();
        
        return item.AsDto();
    }

    // POST /items/{id}
    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostAsync(CreateUpdateItemDto createItemDto)
    {
        var item = new Item
        {
            Name = createItemDto.Name,
            Description = createItemDto.Description,
            Price = createItemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await itemsRepository.CreateAsync(item);

        await publishEndpoint.Publish(new CatalogItemCreated {
            ItemId = item.Id,
            Name = item.Name,
            Description = item.Description
        });

        return CreatedAtAction(nameof(GetByIdAsync), new {id = item.Id}, item);
    }

    // PUT /items/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(Guid id, CreateUpdateItemDto updateItemDto)
    {
        var existingItem = await itemsRepository.GetAsync(id);

        if(existingItem is null)
            return NotFound();

        existingItem.Name = updateItemDto.Name;
        existingItem.Description = updateItemDto.Description;
        existingItem.Price = updateItemDto.Price;

        await itemsRepository.UpdateAsync(existingItem);

        await publishEndpoint.Publish(new CatalogItemUpdated {
            ItemId = existingItem.Id,
            Name = existingItem.Name,
            Description = existingItem.Description
        });

        return NoContent();
    }

    // DELETE /items/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var item = await itemsRepository.GetAsync(id);

        if(item is null)
            return NotFound();

        await itemsRepository.RemoveAsync(item.Id);

        await publishEndpoint.Publish(new CatalogItemDeleted {
            ItemId = id
        });

        return NoContent();
    }
}