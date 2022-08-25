using Shopping.Inventory.Service.Models;
using Shopping.Inventory.Service.Dtos;

namespace Shopping.Inventory.Service;

public static class Extensions
{
    public static InventoryItemDto AsDto(this InventoryItem item, string name, string description)
    {
        return new InventoryItemDto { CatalogItemId = item.CatalogItemId, Name = name, Description = description, Quantity = item.Quantity, AcquiredDate = item.AcquiredDate };
    }
}