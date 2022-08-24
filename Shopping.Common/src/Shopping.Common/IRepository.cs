using System.Linq.Expressions;

namespace Shopping.Common;

public interface IRepository<T> where T : IModel
{
    Task CreateAsync(T item);
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T,bool>> filter);
    Task<T> GetAsync(Guid id);
    Task<T> GetAsync(Expression<Func<T,bool>> filter);
    Task RemoveAsync(Guid id);
    Task UpdateAsync(T item);
}
