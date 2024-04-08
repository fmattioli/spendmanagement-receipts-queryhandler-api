using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using Receipts.ReadModel.Application.Queries.Receipt.GetVariableReceipts;
using SpendManagement.ReadModel.IntegrationTests.Fixtures;
using SpendManagement.ReadModel.IntegrationTests.Helpers;
using System.Net;
using Web.Contracts.Receipt.Requests;

namespace SpendManagement.ReadModel.IntegrationTests.Queries
{
    [Collection(nameof(SharedFixtureCollection))]
    public class ReceiptQueryTests(MongoDBFixture mongoDBFixture) : BaseTests
    {
        private readonly Fixture _fixture = new();
        private readonly MongoDBFixture _mongoDBFixture = mongoDBFixture;

        [Fact]
        private async Task OnGivenAValidGuidsAsReceiptFilter_ShouldBeReturnedAValidReceipts()
        {
            //Arrange
            var receiptId = _fixture.Create<Guid>();

            var receipt = _fixture.Build<Fixtures.Receipt>()
                .With(x => x.Id, receiptId)
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.ReceiptIds, [receiptId])
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getVariableReceipts",
                receiptFilter,
                nameof(receiptFilter.ReceiptIds));

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetVariableReceiptsResponse>(Content);

            receiptResponse?.Results.Should().NotBeNull();
            receiptResponse?.Results.Should().Contain(x => x.Id.Equals(receiptId));
        }

        [Fact]
        private async Task OnGivenAValidReceiptIdGuidsAsReceiptFilter_ShouldBeReturnedAValidReceipts()
        {
            //Arrange
            var receiptItemId = _fixture.Create<Guid>();

            var receiptItem = _fixture.Build<Fixtures.ReceiptItem>()
                .With(x => x.Id, receiptItemId)
                .Create();

            var receipt = _fixture.Build<Fixtures.Receipt>()
                .With(x => x.ReceiptItems, new List<ReceiptItem> { receiptItem })
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.ReceiptIds, [receipt.Id])
                .With(x => x.ReceiptItemIds, [receiptItemId])
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getVariableReceipts",
                receiptFilter,
                nameof(receiptFilter.ReceiptIds));

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetVariableReceiptsResponse>(Content);

            receiptResponse?.Results
                .Should()
                .NotBeNull();

            var foundItems = receiptResponse?
                .Results
                .SelectMany(r => r.ReceiptItems.Where(receiptItem => receiptItem.Id == receiptItemId));

            foundItems.Should().NotBeNull();
        }

        [Fact]
        private async Task OnGivenAValidCategoryIdAsReceiptFilter_ShouldBeReturnedAValidReceipts()
        {
            //Arrange
            var categoryId = _fixture.Create<Guid>();

            var receipt = _fixture.Build<Fixtures.Receipt>()
                .With(x => x.CategoryId, categoryId)
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.CategoryIds, [categoryId])
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getVariableReceipts",
                receiptFilter,
                nameof(receiptFilter.CategoryIds));

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetVariableReceiptsResponse>(Content);

            receiptResponse?.Results
                .Should()
                .NotBeNull();

            receiptResponse?.Results
                .Should()
                .Contain(x => x.Category.Id == categoryId);
        }

        [Fact]
        private async Task OnGivenAValidEstabilishmentNameAsReceiptFilter_ShouldBeReturnedAValidReceipts()
        {
            //Arrange
            var establishmentName = _fixture.Create<string>();

            var receipt = _fixture.Build<Fixtures.Receipt>()
                .With(x => x.EstablishmentName, establishmentName)
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.EstablishmentNames, new List<string> { establishmentName })
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getVariableReceipts",
                receiptFilter,
                nameof(receiptFilter.EstablishmentNames));

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetVariableReceiptsResponse>(Content);

            receiptResponse?.Results.Should().NotBeNull();
            receiptResponse?.Results.Should().Contain(x => x.EstablishmentName.Equals(establishmentName));
        }

        [Fact]
        private async Task OnGivenAValidReceiptDatesAsReceiptFilter_ShouldBeReturnedAValidReceipts()
        {
            //Arrange
            var dateIni = DateTime.UtcNow;
            var dateFinal = DateTime.UtcNow.AddDays(2);

            var receiptOne = _fixture.Build<Fixtures.Receipt>()
                .With(x => x.ReceiptDate, dateIni)
                .Create();

            var receiptTwo = _fixture.Build<Fixtures.Receipt>()
                .With(x => x.ReceiptDate, dateFinal)
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receiptOne, receiptTwo);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.ReceiptDate, dateIni)
                .With(x => x.ReceiptDateFinal, dateFinal)
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getVariableReceipts",
                receiptFilter,
                nameof(receiptFilter.ReceiptDate),
                nameof(receiptFilter.ReceiptDateFinal));

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetVariableReceiptsResponse>(Content);

            receiptResponse?.Results.Should().NotBeNull();
            var results = receiptResponse?.Results.Where(x => x.ReceiptDate >= dateIni && x.ReceiptDate <= dateFinal);
            results.Should().NotBeNull();
        }

        [Fact]
        private async Task OnGivenAValidReceiptItemsGuidsAsReceiptFilter_ShouldBeReturnedAValidReceipts()
        {
            //Arrange
            var receiptItemId = _fixture.Create<Guid>();

            var receiptItem = _fixture.Build<Fixtures.ReceiptItem>()
                .With(x => x.Id, receiptItemId)
                .Create();

            var receipt = _fixture.Build<Fixtures.Receipt>()
                .With(x => x.ReceiptItems, new List<ReceiptItem> { receiptItem })
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.ReceiptItemIds, [receiptItemId])
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getVariableReceipts",
                receiptFilter,
                nameof(receiptFilter.ReceiptItemIds));

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetVariableReceiptsResponse>(Content);

            receiptResponse?.Results
                .Should()
                .NotBeNull();

            var foundItems = receiptResponse?
                .Results
                .SelectMany(r => r.ReceiptItems.Where(receiptItem => receiptItem.Id == receiptItemId));

            foundItems.Should().NotBeNull();
        }

        [Fact]
        private async Task OnGivenAValidReceiptItemsNamesAsReceiptFilter_ShouldBeReturnedAValidReceipts()
        {
            //Arrange
            var receiptItemName = _fixture.Create<string>();

            var receiptItem = _fixture.Build<Fixtures.ReceiptItem>()
                .With(x => x.ItemName, receiptItemName)
                .Create();

            var receipt = _fixture.Build<Fixtures.Receipt>()
                .With(x => x.ReceiptItems, new List<ReceiptItem> { receiptItem })
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.ReceiptItemNames, new List<string> { receiptItemName })
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getVariableReceipts",
                receiptFilter,
                nameof(receiptFilter.ReceiptItemNames));

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetVariableReceiptsResponse>(Content);

            receiptResponse?.Results
                .Should()
                .NotBeNull();

            var foundItems = receiptResponse?
                .Results
                .SelectMany(r => r.ReceiptItems.Where(receiptItem => receiptItem.ItemName == receiptItemName));

            foundItems.Should().NotBeNull();
        }

        [Fact]
        private async Task OnGivenAValidReceiptIdGuidsAsRecurringReceiptFilter_ShouldBeReturnedAValidReceipts()
        {
            //Arrange
            var receiptId = _fixture.Create<Guid>();

            var receipt = _fixture.Build<RecurringReceipt>()
                .Create();

            await _mongoDBFixture.InsertRecurringReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.ReceiptItemIds, [receiptId])
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getRecurringReceipts",
                receiptFilter,
                nameof(receiptFilter.ReceiptIds));

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetVariableReceiptsResponse>(Content);

            receiptResponse?.Results
                .Should()
                .NotBeNull();

            var foundItems = receiptResponse?
                .Results
                .SelectMany(r => r.ReceiptItems.Where(receiptItem => receiptItem.Id == receiptId));

            foundItems.Should().NotBeNull();
        }

        [Fact]
        private async Task OnGivenAValidCategoryIdAsRecurringReceiptFilter_ShouldBeReturnedAValidReceipts()
        {
            //Arrange
            var categoryId = _fixture.Create<Guid>();

            var receipt = _fixture.Build<RecurringReceipt>()
                .With(x => x.CategoryId, categoryId)
                .Create();

            await _mongoDBFixture.InsertRecurringReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.CategoryIds, [categoryId])
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getRecurringReceipts",
                receiptFilter,
                nameof(receiptFilter.CategoryIds));

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetVariableReceiptsResponse>(Content);

            receiptResponse?.Results.Should().NotBeNull();
            receiptResponse?.Results.Should().Contain(x => x.Category.Id.Equals(categoryId));
        }

        [Fact]
        private async Task OnGivenAValidEstablishmentNameAsRecurringReceiptFilter_ShouldBeReturnedAValidReceipts()
        {
            //Arrange
            var establishmentName = _fixture.Create<string>();

            var receipt = _fixture.Build<RecurringReceipt>()
                .With(x => x.EstablishmentName, establishmentName)
                .Create();

            await _mongoDBFixture.InsertRecurringReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.EstablishmentNames, new List<string> { establishmentName })
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getRecurringReceipts",
                receiptFilter,
                nameof(receiptFilter.EstablishmentNames));

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetVariableReceiptsResponse>(Content);

            receiptResponse?.Results.Should().NotBeNull();
            receiptResponse?.Results.Should().Contain(x => x.EstablishmentName.Equals(establishmentName));
        }
    }
}
