namespace Shopping.Inventory.Service.Dtos;

public record CatalogItemDto
{
    public Guid Id { get; set;}
    public string ?Name { get; set;}
    public string ?Description { get; set;}
}