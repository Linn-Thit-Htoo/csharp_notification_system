namespace notification_system.notification.Persistence.Base;

public interface IRepositoryBase<T>
    where T : class
{
    IQueryable<T> GetAll();
    IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
    IQueryable<T> Query(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? order = null
    );
    void Add(T entity);
    void Add(IEnumerable<T> entities);
    Task AddAsync(T entity, CancellationToken cs = default);
    Task AddAsync(IEnumerable<T> entities, CancellationToken cs = default);
    void Update(T entity);
    void Update(IEnumerable<T> entities);
    void Delete(T entity);
    void Delete(IEnumerable<T> entities);
    void Dispose();
}
