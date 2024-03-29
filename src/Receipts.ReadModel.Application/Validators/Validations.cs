using Application.Validators;

namespace Application.Extensions
{
    public static class Validations
    {
        public static E ValidateIfEntityIsValid<E>(this E entity)
        {
            if (Equals(entity, default(E)))
            {
                throw new NotFoundException($"Any {typeof(E).Name} was found. ");
            }

            return entity;
        }
    }
}
