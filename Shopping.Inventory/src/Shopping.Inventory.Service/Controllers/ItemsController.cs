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
    private readonly IRepository<InventoryItem> inventoryItemsRepository;
    private readonly IRepository<CatalogItem> catalogItemsRepository;

    public ItemController(IRepository<InventoryItem> inventoryItemsRepository, IRepository<CatalogItem> catalogItemsRepository)
    {
        this.inventoryItemsRepository = inventoryItemsRepository;
        this.catalogItemsRepository = catalogItemsRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
    {
        if(userId == Guid.Empty)
            return BadRequest();

        var inventoryItemsEntities = await inventoryItemsRepository.GetAllAsync(item => item.UserId == userId);
        var itemIds = inventoryItemsEntities.Select(item => item.CatalogItemId);
        var catalogItemEntities = await catalogItemsRepository.GetAllAsync(item => itemIds.Contains(item.Id));

        var inventoryItemDtos = inventoryItemsEntities.Select(inventoryItem =>
        {
            var catalogItem = catalogItemEntities.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
            return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
        });

        return Ok(inventoryItemDtos);
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
    {
        var inventoryItem = await inventoryItemsRepository.GetAsync(
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

            await inventoryItemsRepository.CreateAsync(inventoryItem);
        }
        else
        {
            inventoryItem.Quantity += grantItemsDto.Quantity;
            await inventoryItemsRepository.UpdateAsync(inventoryItem);
        }

        return Ok();
    }
}