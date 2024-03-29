using Application.Queries.Receipt.GetVariableReceipts;
using AutoFixture;
using Application.Converters;
using FluentAssertions;
using Receipts.ReadModel;
using Receipts.ReadModel.QueriesFilters.PageFilters;
using Application.Queries.Common;
using Receipts.ReadModel.Entities;

namespace SpendManagement.ReadModel.UnitTests.Mappers
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
            var result = receiptFilters?.ToDomainFilters();

            // Assert
            result.Should().BeEquivalentTo(receiptFilters, options =>
                options.Excluding(x => x!.PageFilter)
                );
        }

        [Fact]
        public void ToResponse_WhenCalled_ReturnsEquivalentGetReceiptsResponse()
        {
            // Arrange
            var receiptsPagedFilter = _fixture.Create<PagedResultFilter<Receipt>>();

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
                .Should()
                .BeEquivalentTo(receiptsPagedFilter, options => options
                    .Excluding(x => x.ReceiptsTotalAmount));
        }

        [Fact]
        public void ToReceiptResponse_WhenCalled_ReturnsEquivalentReceiptResponse()
        {
            // Arrange
            var receipt = _fixture.Create<Receipt>();

            // Act
            var result = receipt.ToReceiptResponse();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(receipt);
        }
    }
}
