using Application.Queries.Receipt.GetReceipts;
using AutoFixture;
using Domain.Interfaces;
using Domain.Queries.GetReceipts;
using Domain.QueriesFilters.PageFilters;
using Moq;

namespace SpendManagament.ReadModel.UnitTests.Queries.Receipt
{
    public class ReceiptQueriesTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IReceiptRepository> mockReceiptRepository = new();
        private readonly GetReceiptsQueryHandler _receiptsQueryHandler;

        public ReceiptQueriesTests()
        {
            _receiptsQueryHandler = new GetReceiptsQueryHandler(mockReceiptRepository.Object);
        }

        [Fact]
        public async Task OnGivenAValidFilter_ShouldBeReturnedReceiptsFromDatabase()
        {
            //Arrange
            var filter = _fixture.Create<GetReceiptsQuery>();

            var receipts = _fixture.Create<PagedResultFilter<Domain.Entities.Receipt>>();

            mockReceiptRepository
                .Setup(x => x.GetReceiptsAsync(It.IsAny<ReceiptFilters>()))
                .Returns(Task.FromResult(receipts));

            //Act
            await _receiptsQueryHandler.Handle(filter, CancellationToken.None);

            //Assert
            mockReceiptRepository.Verify(x => x.GetReceiptsAsync(It.IsAny<ReceiptFilters>()), Times.Once);
            mockReceiptRepository.VerifyNoOtherCalls();
        }
    }
}
