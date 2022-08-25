using Shopping.Common;

namespace Shopping.Inventory.Service.Models;

public class InventoryItem : IModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CatalogItemId { get; set; }
    public int Quantity { get; set; }
    public DateTimeOffset AcquiredDate { get; set; }
}