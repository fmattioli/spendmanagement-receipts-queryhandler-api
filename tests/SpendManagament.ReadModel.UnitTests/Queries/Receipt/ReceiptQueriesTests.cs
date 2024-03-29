using Application.Queries.Receipt.GetRecurringReceipts;
using Application.Queries.Receipt.GetVariableReceipts;
using AutoFixture;
using Receipts.ReadModel.Interfaces;
using Receipts.ReadModel.Queries.GetReceipts;
using Receipts.ReadModel.QueriesFilters;
using Receipts.ReadModel.QueriesFilters.PageFilters;
using Moq;
using Receipts.ReadModel.Entities;

namespace SpendManagement.ReadModel.UnitTests.Queries.Receipt
{
    public class ReceiptQueriesTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IReceiptRepository> mockReceiptRepository = new();
        private readonly GetVariableReceiptsQueryHandler _receiptsQueryHandler;
        private readonly GetRecurringReceiptsQueryHandler _recurringReceiptsQueryHandler;

        public ReceiptQueriesTests()
        {
            _receiptsQueryHandler = new GetVariableReceiptsQueryHandler(mockReceiptRepository.Object);
            _recurringReceiptsQueryHandler = new GetRecurringReceiptsQueryHandler(mockReceiptRepository.Object);
        }

        [Fact]
        public async Task OnGivenAValidFilter_ShouldBeReturnedReceiptsFromDatabase()
        {
            //Arrange
            var filter = _fixture.Create<GetVariableReceiptsQuery>();

            var receipts = _fixture.Create<PagedResultFilter<Receipts.ReadModel.Entities.Receipt>>();

            mockReceiptRepository
                .Setup(x => x.GetVariableReceiptsAsync(It.IsAny<ReceiptFilters>()))
                .Returns(Task.FromResult(receipts));

            //Act
            await _receiptsQueryHandler.Handle(filter, CancellationToken.None);

            //Assert
            mockReceiptRepository.Verify(x => x.GetVariableReceiptsAsync(It.IsAny<ReceiptFilters>()), Times.Once);
            mockReceiptRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task OnGivenAValidFilter_ShouldBeReturnedRecurringReceiptsFromDatabase()
        {
            //Arrange
            var filter = _fixture.Create<GetRecurringReceiptsQuery>();

            var receipts = _fixture.Create<PagedResultFilter<RecurringReceipt>>();

            mockReceiptRepository
                .Setup(x => x.GetRecurringReceiptsAsync(It.IsAny<RecurringReceiptFilters>()))
                .Returns(Task.FromResult(receipts));

            //Act
            await _recurringReceiptsQueryHandler.Handle(filter, CancellationToken.None);

            //Assert
            mockReceiptRepository.Verify(x => x.GetRecurringReceiptsAsync(It.IsAny<RecurringReceiptFilters>()), Times.Once);
            mockReceiptRepository.VerifyNoOtherCalls();
        }
    }
}
