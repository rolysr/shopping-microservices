using Shopping.Catalog.Service.Models;

namespace Shopping.Catalog.Service.Repositories;

public interface IItemsRepository
{
    Task CreateAsync(Item item);
    Task<IReadOnlyCollection<Item>> GetAllAsync();
    Task<Item> GetAsync(Guid id);
    Task RemoveAsync(Guid id);
    Task UpdateAsync(Item item);
}
