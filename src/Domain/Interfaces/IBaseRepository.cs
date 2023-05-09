using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class 
    {
        //Task<TEntity> FindOneAsync(
        //    Expression<Func<TEntity, bool>> filterExpression);
    }
}
