using AutoFixture;
using Contracts.Web.Http.Common;
using Contracts.Web.Http.Receipt.Requests;
using Contracts.Web.Models;

using FluentAssertions;
using Receipts.QueryHandler.Application.Mappers;
using Receipts.QueryHandler.Domain.Entities;
using Receipts.QueryHandler.Domain.QueriesFilters.PageFilters;

namespace Receipts.QueryHandler.UnitTests.Mappers
{
    public class ReceiptMapperTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void ToDomainFilters_WhenCalled_ReturnsEquivalentReceiptFilters()
        {
            // Arrange
            var receiptFilters = _fixture.Create<GetVariableReceiptsRequest>();

            // Act
            var result = receiptFilters?.ToDomainFilters(_fixture.Create<AuthModel>());

            // Assert
            result.Should().BeEquivalentTo(receiptFilters, options =>
                options.Excluding(x => x!.PageFilter)
                );
        }

        [Fact]
        public void ToResponse_WhenCalled_ReturnsEquivalentGetReceiptsResponse()
        {
            // Arrange
            var receipts = _fixture.Create<VariableReceipt>();
            var receiptsPagedFilter = _fixture.Create<PagedResultFilter<VariableReceipt>>();

            // Act
            var result = receiptsPagedFilter.ToResponse(new PageFilterRequest
            {
                PageSize = receiptsPagedFilter.PageSize,
            });

            // Assert
            result
                .Should()
                .NotBeNull();

            result
                .Results.First()
                .Should()
                .BeEquivalentTo(receiptsPagedFilter.Results.First(), x => x
                    .Excluding(x => x.Category.Tenant)
                    .Excluding(x => x.Category.UserId)
                    .Excluding(x => x.UserId)
                    .Excluding(x => x.Tenant));
        }

        [Fact]
        public void ToReceiptResponse_WhenCalled_ReturnsEquivalentReceiptResponse()
        {
            // Arrange
            var receipt = _fixture.Create<VariableReceipt>();

            // Act
            var result = receipt.ToReceiptResponse();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(receipt, 
                x => x
                .Excluding(x => x.Tenant)
                .Excluding(x => x.Category.Tenant)
                .Excluding(x => x.Category.UserId)
                .Excluding(x => x.UserId));
        }
    }
}
