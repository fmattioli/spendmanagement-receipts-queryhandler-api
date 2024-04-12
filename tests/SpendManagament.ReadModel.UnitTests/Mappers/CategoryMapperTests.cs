using AutoFixture;
using Contracts.Web.Category.Requests;
using Contracts.Web.Common;
using FluentAssertions;
using Receipts.ReadModel.Application.Mappers;
using Receipts.ReadModel.Entities;
using Receipts.ReadModel.QueriesFilters.PageFilters;
namespace SpendManagement.ReadModel.UnitTests.Mappers
{
    public class CategoryMapperTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void ToCategoryResponse_WhenCalled_ReturnsEquivalentCategoryResponse()
        {
            // Arrange
            var category = _fixture.Create<Category>();

            // Act
            var result = category.ToCategoryResponse();

            // Assert
            result.Should().BeEquivalentTo(category);
        }

        [Fact]
        public void ToDomainFilters_WhenCalled_ReturnsEquivalentCategoryResponse()
        {
            // Arrange
            var getCategoriesRequest = _fixture.Create<GetCategoriesRequest>();

            // Act
            var result = getCategoriesRequest.ToDomainFilters();

            // Assert
            result.Should().NotBeNull();
            result.CategoryIds.Should().BeEquivalentTo(getCategoriesRequest.CategoryIds);
            result.CategoryNames.Should().BeEquivalentTo(getCategoriesRequest.CategoryNames);
            result.PageNumber.Should().Be(getCategoriesRequest.PageFilter.Page);
            result.PageSize.Should().Be(getCategoriesRequest.PageFilter.PageSize);
        }

        [Fact]
        public void ToResponse_WhenCalled_ReturnsEquivalentCategoryResponse()
        {
            // Arrange
            var categoriesPagedFilter = _fixture.Create<PagedResultFilter<Category>>();

            // Act
            var result = categoriesPagedFilter.ToResponse(new PageFilterRequest
            {
                Page = 1,
                PageSize = categoriesPagedFilter.PageSize
            });

            // Assert
            result.Should().NotBeNull();
            result.Results.Should().HaveCount(categoriesPagedFilter.Results.Count());
            result.TotalResults.Should().Be(categoriesPagedFilter.TotalResults);
            result.TotalPages.Should().Be(categoriesPagedFilter.TotalPages);
            result.PageSize.Should().Be(categoriesPagedFilter.PageSize);
        }
    }
}
