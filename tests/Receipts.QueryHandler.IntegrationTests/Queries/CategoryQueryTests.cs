using AutoFixture;
using Contracts.Web.Http.Category.Requests;
using FluentAssertions;
using Newtonsoft.Json;
using Receipts.QueryHandler.Application.Queries.Category.GetCategories;
using Receipts.QueryHandler.Domain.Entities;
using Receipts.QueryHandler.IntegrationTests.Fixtures;
using System.Net;

namespace Receipts.QueryHandler.IntegrationTests.Queries
{
    [Collection(nameof(SharedFixtureCollection))]
    public class CategoryQueryTests(MongoDBFixture mongoDBFixture, HttpFixture httpFixture)
    {
        private readonly Fixture _fixture = new();
        private readonly MongoDBFixture _mongoDBFixture = mongoDBFixture;
        private readonly HttpFixture _httpFixture = httpFixture;

        [Fact]
        private async Task OnGivenAValidGuidsAsCategoryFilter_ShouldBeReturnedAValidCategory()
        {
            //Arrange
            var categoryId = _fixture.Create<Guid>();

            var tenant = _fixture
                .Build<Tenant>()
                .With(x => x.Number, 10000)
                .Create();

            var category = _fixture.Build<Category>()
                .With(x => x.Id, categoryId)
                .With(x => x.Tenant, tenant)
                .Create();

            await _mongoDBFixture.InsertCategoryAsync(category);

            var categoryFilter = _fixture
                .Build<GetCategoriesRequest>()
                .With(x => x.CategoryIds, [categoryId])
                .Create();

            //Act
            var (StatusCode, Content) = await _httpFixture.GetAsync("/getCategories", categoryFilter, nameof(categoryFilter.CategoryIds));

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var categoriesResponse = JsonConvert.DeserializeObject<GetCategoriesResponse>(Content);

            categoriesResponse?.Results.Should().NotBeNull();
            categoriesResponse?.Results.Should().Contain(x => x.Id.Equals(categoryId));
        }

        [Fact]
        private async Task OnGivenAValidNamesAsCategoryFilter_ShouldBeReturnedAValidCategory()
        {
            //Arrange
            var categoryName = _fixture.Create<string>();

            var tenant = _fixture
                .Build<Tenant>()
                .With(x => x.Number, 10000)
                .Create();

            var category = _fixture.Build<Category>()
                .With(x => x.Name, categoryName)
                .With(x => x.Tenant, tenant)
                .Create();

            await _mongoDBFixture.InsertCategoryAsync(category);

            var categoryFilter = _fixture
                .Build<GetCategoriesRequest>()
                .With(x => x.CategoryNames, new List<string> { categoryName })
                .Without(x => x.CategoryIds)
                .Create();

            //Act
            var (StatusCode, Content) = await _httpFixture.GetAsync("/getCategories", categoryFilter, nameof(categoryFilter.CategoryNames));

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var categoriesResponse = JsonConvert.DeserializeObject<GetCategoriesResponse>(Content);

            categoriesResponse?.Results.Should().NotBeNull();
            categoriesResponse?.Results.Should().Contain(x => x.Name!.Equals(categoryName));
        }
    }
}