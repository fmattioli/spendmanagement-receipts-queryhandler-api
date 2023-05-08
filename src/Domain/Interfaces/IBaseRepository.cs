using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class 
    {
        Task<TEntity> FindOneAsync(
            Expression<Func<TEntity, bool>> filterExpression);
    }
}
