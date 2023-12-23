using Application.Queries.Category.GetCategories;
using AutoFixture;
using Domain.Interfaces;
using Domain.QueriesFilters;
using Domain.QueriesFilters.PageFilters;
using Moq;

namespace SpendManagament.ReadModel.UnitTests.Queries.Category
{
    public class CategoryQueriesTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<ICategoryRepository> mockCategoryRepository = new();
        private readonly GetCategoriesQueryHandler _categoriesQueryHandler;

        public CategoryQueriesTests()
        {
            _categoriesQueryHandler = new GetCategoriesQueryHandler(mockCategoryRepository.Object);
        }

        [Fact]
        public async Task OnGivenAValidFilter_ShouldBeReturnedCategoriesFromDatabase()
        {
            //Arrange
            var filter = _fixture.Create<GetCategoriesQuery>();

            var categories = _fixture.Create<PagedResultFilter<Domain.Entities.Category>>();

            mockCategoryRepository
                .Setup(x => x.GetCategoriesAsync(It.IsAny<CategoryFilters>()))
                .Returns(Task.FromResult(categories));

            //Act
            await _categoriesQueryHandler.Handle(filter, CancellationToken.None);

            //Assert
            mockCategoryRepository.Verify(x => x.GetCategoriesAsync(It.IsAny<CategoryFilters>()), Times.Once);
            mockCategoryRepository.VerifyNoOtherCalls();
        }
    }
}
