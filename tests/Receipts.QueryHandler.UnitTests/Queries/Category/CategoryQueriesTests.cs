using AutoFixture;
using Contracts.Web.Services.Auth;
using Moq;
using Receipts.QueryHandler.Application.Queries.Category.GetCategories;
using Receipts.QueryHandler.Domain.Interfaces;
using Receipts.QueryHandler.Domain.QueriesFilters;
using Receipts.QueryHandler.Domain.QueriesFilters.PageFilters;

namespace Receipts.QueryHandler.UnitTests.Queries.Category
{
    public class CategoryQueriesTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<ICategoryRepository> mockCategoryRepository = new();
        private readonly Mock<IAuthService> authServiceRepository = new();
        private readonly GetCategoriesQueryHandler _categoriesQueryHandler;

        public CategoryQueriesTests()
        {
            _categoriesQueryHandler = new GetCategoriesQueryHandler(mockCategoryRepository.Object, authServiceRepository.Object);
        }

        [Fact]
        public async Task OnGivenAValidFilter_ShouldBeReturnedCategories()
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
