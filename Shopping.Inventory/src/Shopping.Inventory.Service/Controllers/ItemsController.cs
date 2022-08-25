using Shopping.Common;
using Shopping.Inventory.Service.Models;
using Shopping.Inventory.Service.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Shopping.Inventory.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemController : ControllerBase
{
    private readonly IRepository<InventoryItem> itemsRepository;

    public ItemController(IRepository<InventoryItem> itemsRepository)
    {
        this.itemsRepository = itemsRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
    {
        if(userId == Guid.Empty)
            return BadRequest();

        var items = (await itemsRepository.GetAllAsync(item => item.UserId == userId))
                    .Select(item => item.AsDto());
        
        return Ok(items);
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