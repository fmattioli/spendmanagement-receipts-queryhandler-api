using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> FindOneAsync(
            Expression<Func<T, bool>> filterExpression);
    }
}
