namespace Shopping.Catalog.Contracts;

public record CatalogItemUpdated
{
    public Guid ItemId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
