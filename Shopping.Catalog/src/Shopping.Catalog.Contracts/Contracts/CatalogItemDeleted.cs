namespace Shopping.Catalog.Contracts;

public record CatalogItemDeleted
{
    public Guid ItemId { get; set; }
}