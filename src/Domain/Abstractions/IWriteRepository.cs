using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions;

public interface IWriteRepository<TEntity, in TKey> where TEntity : class
{
    Task AddAsync(TEntity entity , CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities , CancellationToken ct = default);

    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    Task RemoveByIdAsync(TKey id , CancellationToken ct = default);
}
