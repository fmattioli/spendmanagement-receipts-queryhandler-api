using Domain.Entities;
using Web.Contracts.Category;

namespace Application.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryResponse ToCategoryResponse(this Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                CreatedDate = category.CreatedDate,
                Name = category.Name,
            };
        }
    }
}
