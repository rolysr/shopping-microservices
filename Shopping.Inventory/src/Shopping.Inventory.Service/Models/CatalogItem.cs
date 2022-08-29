using Shopping.Common;

namespace Shopping.Inventory.Service.Models;

public class CatalogItem : IModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}