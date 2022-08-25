using Shopping.Inventory.Service.Models;
using Shopping.Inventory.Service.Dtos;

namespace Shopping.Inventory.Service;

public static class Extensions
{
    public static InventoryItemDto AsDto(this InventoryItem item)
    {
        return new InventoryItemDto { CatalogItemId = item.CatalogItemId, Quantity = item.Quantity, AcquiredDate = item.AcquiredDate };
    }
}