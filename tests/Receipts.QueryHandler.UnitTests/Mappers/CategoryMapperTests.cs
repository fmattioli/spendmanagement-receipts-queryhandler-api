﻿using AutoFixture;
using Contracts.Web.Http.Category.Requests;
using Contracts.Web.Http.Common;
using FluentAssertions;
using Receipts.QueryHandler.Application.Mappers;
using Receipts.QueryHandler.Domain.Entities;
using Receipts.QueryHandler.Domain.QueriesFilters.PageFilters;
namespace Receipts.QueryHandler.UnitTests.Mappers
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
            result.Should().BeEquivalentTo(category, x =>
                x.Excluding(x => x.Tenant)
                .Excluding(x => x.UserId));
        }

        [Fact]
        public void ToDomainFilters_WhenCalled_ReturnsEquivalentCategoryResponse()
        {
            // Arrange
            var getCategoriesRequest = _fixture.Create<GetCategoriesRequest>();
            var userId = _fixture.Create<Guid>();
            var tenantId = _fixture.Create<int>();

            // Act
            var result = getCategoriesRequest.ToDomainFilters(userId, tenantId);

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
