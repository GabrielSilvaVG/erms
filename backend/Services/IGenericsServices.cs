namespace ERMS.Services
{
    /// <summary>
    /// Generic service interface for CRUD operations with async support.
    /// TEntity: The entity type (User, Event, Registration) - must be a class
    /// TKey: The primary key type (usually int) - must be comparable (IEquatable) - int, string, etc.
    /// All methods use Task for asynchronous operations to improve API performance
    /// </summary>
    public interface IGenericService<TEntity, TKey> 
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TKey id);
    }
}