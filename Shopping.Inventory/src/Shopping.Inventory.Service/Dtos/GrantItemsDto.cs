namespace Shopping.Inventory.Service.Dtos;

public record GrantItemsDto
{
    public Guid UserId { get; set; }
    public Guid CatalogItemId { get; set; }
    public int Quantity { get; set; }
}