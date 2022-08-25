using Shopping.Common;
using Shopping.Inventory.Service.Models;
using Shopping.Inventory.Service.Dtos;
using Shopping.Inventory.Service.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Shopping.Inventory.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemController : ControllerBase
{
    private readonly IRepository<InventoryItem> itemsRepository;
    private readonly CatalogClient catalogClient;

    public ItemController(IRepository<InventoryItem> itemsRepository, CatalogClient catalogClient)
    {
        this.itemsRepository = itemsRepository;
        this.catalogClient = catalogClient;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
    {
        if(userId == Guid.Empty)
            return BadRequest();

        var catalogItems = await catalogClient.GetCatalogItemsAsync();
        var inventoryItemsEntities = await itemsRepository.GetAllAsync(item => item.UserId == userId);
        
        var inventoryItemDtos = inventoryItemsEntities.Select(inventoryItem =>
        {
            var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
            return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
        });

        return Ok(inventoryItemDtos);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
    {
        var inventoryItem = await itemsRepository.GetAsync(
            item => item.UserId == grantItemsDto.UserId && item.CatalogItemId == grantItemsDto.CatalogItemId);

        if(inventoryItem is null)
        {
            inventoryItem = new InventoryItem
            {
                CatalogItemId = grantItemsDto.CatalogItemId,
                UserId = grantItemsDto.UserId,
                Quantity = grantItemsDto.Quantity,
                AcquiredDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(inventoryItem);
        }
        else
        {
            inventoryItem.Quantity += grantItemsDto.Quantity;
            await itemsRepository.UpdateAsync(inventoryItem);
        }

        return Ok();
    }
}