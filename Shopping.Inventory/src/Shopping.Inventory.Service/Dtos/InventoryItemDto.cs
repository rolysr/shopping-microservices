namespace Shopping.Inventory.Service.Dtos;

public record InventoryItemDto
{
    public Guid CatalogItemId { get; set; }
    public int Quantity { get; set; }
    public DateTimeOffset AcquiredDate { get; set; }
}