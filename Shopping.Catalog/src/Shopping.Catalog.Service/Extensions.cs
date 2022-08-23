using Shopping.Catalog.Service.Dtos;
using Shopping.Catalog.Service.Models;

namespace Shopping.Catalog.Service;

public static class Extensions
{
    public static ItemDto AsDto(this Item item)
    {
        return new ItemDto { Id = item.Id, Name = item.Name, Description = item.Description, Price = item.Price, CreatedDate = item.CreatedDate };
    }
}

