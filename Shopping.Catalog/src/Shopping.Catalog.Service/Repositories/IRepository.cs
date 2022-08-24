using Shopping.Catalog.Service.Models;

namespace Shopping.Catalog.Service.Repositories;

public interface IRepository<T> where T : IModel
{
    Task CreateAsync(T item);
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<T> GetAsync(Guid id);
    Task RemoveAsync(Guid id);
    Task UpdateAsync(T item);
}
