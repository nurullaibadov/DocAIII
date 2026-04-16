using DocAi.Core.Entities;

namespace DocAi.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : BaseEntity;
    Task<int> CommitAsync();
}
