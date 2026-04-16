using DocAi.Core.Entities;
using DocAi.Core.Interfaces;

namespace DocAi.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Dictionary<Type, object> _repositories;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    public IGenericRepository<T> Repository<T>() where T : BaseEntity
    {
        if (_repositories.ContainsKey(typeof(T)))
            return (IGenericRepository<T>)_repositories[typeof(T)];

        var repository = new GenericRepository<T>(_context);
        _repositories.Add(typeof(T), repository);
        return repository;
    }

    public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}
