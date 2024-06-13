using AutoFixture;
using Moq;
using Receipts.QueryHandler.Application.Providers;
using Receipts.QueryHandler.Application.Queries.Receipt.GetRecurringReceipts;
using Receipts.QueryHandler.Application.Queries.Receipt.GetVariableReceipts;
using Receipts.QueryHandler.Domain.Entities;
using Receipts.QueryHandler.Domain.Interfaces;
using Receipts.QueryHandler.Domain.QueriesFilters;
using Receipts.QueryHandler.Domain.QueriesFilters.PageFilters;

namespace Receipts.QueryHandler.UnitTests.Queries.Receipt
{
    public class ReceiptQueriesTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IReceiptRepository> mockReceiptRepository = new();
        private readonly Mock<IAuthProvider> authProviderRepository = new();
        private readonly GetVariableReceiptsQueryHandler _receiptsQueryHandler;
        private readonly GetRecurringReceiptsQueryHandler _recurringReceiptsQueryHandler;

        public ReceiptQueriesTests()
        {
            _receiptsQueryHandler = new GetVariableReceiptsQueryHandler(mockReceiptRepository.Object, authProviderRepository.Object);
            _recurringReceiptsQueryHandler = new GetRecurringReceiptsQueryHandler(mockReceiptRepository.Object, authProviderRepository.Object);
        }

        [Fact]
        public async Task OnGivenAValidFilter_ShouldBeReturnedReceiptsFromDatabase()
        {
            //Arrange
            var filter = _fixture.Create<GetVariableReceiptsQuery>();

            var receipts = _fixture.Create<PagedResultFilter<Domain.Entities.Receipt>>();

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
                .ReturnsAsync(receipts);

            //Act
            await _recurringReceiptsQueryHandler.Handle(filter, CancellationToken.None);

            //Assert
            mockReceiptRepository.Verify(x => x.GetRecurringReceiptsAsync(It.IsAny<RecurringReceiptFilters>()), Times.Once);
            mockReceiptRepository.VerifyNoOtherCalls();
        }
    }
}
