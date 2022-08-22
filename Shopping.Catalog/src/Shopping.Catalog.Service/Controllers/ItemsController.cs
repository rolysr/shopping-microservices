using Microsoft.AspNetCore.Mvc;
using Shopping.Catalog.Service.Dtos;

namespace Shopping.Catalog.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private static readonly List<ItemDto> items = new()
    {
        new ItemDto{ Id = Guid.NewGuid(), Name = "Balenciaga T-Shirt", Description = "Simple Balenciaga T-Shirt for any event", Price = 80, CreatedDate = DateTimeOffset.UtcNow },
        new ItemDto{ Id = Guid.NewGuid(), Name = "Nike Air Force 1", Description = "Nike shoes for any event", Price = 163, CreatedDate = DateTimeOffset.UtcNow },
        new ItemDto{ Id = Guid.NewGuid(), Name = "Play Station 5", Description = "Ultimate console for best rated videogames", Price = 500, CreatedDate = DateTimeOffset.UtcNow }
    };

    // GET /items
    [HttpGet]
    public IEnumerable<ItemDto> Get()
    {
        return items;
    }

    // GET /items/{id}
    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetById(Guid id)
    {
        var item = items.Where(item => item.Id == id).SingleOrDefault();
        
        if(item is null)
            return NotFound();
        
        return item;
    }

    // POST /items/{id}
    [HttpPost]
    public ActionResult<ItemDto> Post(CreateUpdateItemDto createItemDto)
    {
        var item = new ItemDto{ Id = Guid.NewGuid(), Name = createItemDto.Name, Description = createItemDto.Description, Price = createItemDto.Price, CreatedDate = DateTimeOffset.UtcNow };
        items.Add(item);

        return CreatedAtAction(nameof(GetById), new {id = item.Id}, item);
    }

    // PUT /items/{id}
    [HttpPut("{id}")]
    public IActionResult Put(Guid id, CreateUpdateItemDto updateItemDto)
    {
        var existingItem = items.Where(item => item.Id == id).SingleOrDefault();

        if(existingItem is null)
            return NotFound();

        var updateItem = existingItem with {
            Name = updateItemDto.Name,
            Description = updateItemDto.Description,
            Price = updateItemDto.Price,
        };

        var index = items.FindIndex(existingItem => existingItem.Id == id);
        
        return NoContent();
    }

    // DELETE /items/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var index = items.FindIndex(item => item.Id == id);
        
        if(index < 0)
            return NotFound();
        
        items.RemoveAt(index);

        return NoContent();
    }
}