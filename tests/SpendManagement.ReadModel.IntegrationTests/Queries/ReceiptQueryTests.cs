using Application.Queries.Receipt.GetReceipts;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SpendManagement.ReadModel.IntegrationTests.Fixtures;
using SpendManagement.ReadModel.IntegrationTests.Helpers;
using System.Net;

namespace SpendManagement.ReadModel.IntegrationTests.Queries
{
    [Collection(nameof(SharedFixtureCollection))]
    public class ReceiptQueryTests(MongoDBFixture mongoDBFixture) : BaseTests
    {
        private readonly Fixture _fixture = new();
        private readonly MongoDBFixture _mongoDBFixture = mongoDBFixture;

        [Fact]
        private async Task OnGivenAValidGuidsAsReceiptFilter_ShouldBeReturnedAValidReceiptsFromDataBase()
        {
            //Arrange
            var receiptId = _fixture.Create<Guid>();

            var receipt = _fixture.Build<Fixtures.Receipt>()
                .With(x => x.Id, receiptId)
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetReceiptsRequest>()
                .With(x => x.ReceiptIds, new List<Guid> { receiptId })
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getReceipts",
                receiptFilter,
                [
                    nameof(receiptFilter.ReceiptIds)
                ]);

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetReceiptsResponse>(Content);

            receiptResponse?.Results.Should().NotBeNull();
            receiptResponse?.Results.Should().Contain(x => x.Id.Equals(receiptId));
        }

        [Fact]
        private async Task OnGivenAValidCategoryIdAsReceiptFilter_ShouldBeReturnedAValidReceiptsFromDataBase()
        {
            //Arrange
            var categoryId = _fixture.Create<Guid>();

            var receipt = _fixture.Build<Fixtures.Receipt>()
                .With(x => x.CategoryId, categoryId)
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetReceiptsRequest>()
                .With(x => x.CategoryIds, new List<Guid> { categoryId })
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getReceipts",
                receiptFilter,
                [
                    nameof(receiptFilter.CategoryIds)
                ]);

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetReceiptsResponse>(Content);

            receiptResponse?.Results.Should().NotBeNull();
            receiptResponse?.Results.Should().Contain(x => x.Id.Equals(categoryId));
        }

        [Fact]
        private async Task OnGivenAValidEstabilishmentNameAsReceiptFilter_ShouldBeReturnedAValidReceiptsFromDataBase()
        {
            //Arrange
            var establishmentName = _fixture.Create<string>();

            var receipt = _fixture.Build<Fixtures.Receipt>()
                .With(x => x.EstablishmentName, establishmentName)
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetReceiptsRequest>()
                .With(x => x.EstablishmentNames, new List<string> { establishmentName })
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getReceipts",
                receiptFilter,
                [
                    nameof(receiptFilter.EstablishmentNames)
                ]);

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetReceiptsResponse>(Content);

            receiptResponse?.Results.Should().NotBeNull();
            receiptResponse?.Results.Should().Contain(x => x.EstablishmentName.Equals(establishmentName));
        }

        [Fact]
        private async Task OnGivenAValidReceiptDatesAsReceiptFilter_ShouldBeReturnedAValidReceiptsFromDataBase()
        {
            //Arrange
            var dateIni = DateTime.UtcNow;
            var dateFinal = DateTime.UtcNow.AddDays(2);

            var establishmentName = _fixture.Create<string>();

            var receiptOne = _fixture.Build<Fixtures.Receipt>()
                .With(x => x.ReceiptDate, dateIni)
                .Create();

            var receiptTwo = _fixture.Build<Fixtures.Receipt>()
                .With(x => x.ReceiptDate, dateFinal)
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receiptOne, receiptTwo);

            var receiptFilter = _fixture
                .Build<GetReceiptsRequest>()
                .With(x => x.ReceiptDate, dateIni)
                .With(x => x.ReceiptDateFinal, dateFinal)
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getReceipts",
                receiptFilter,
                [
                    nameof(receiptFilter.ReceiptDate),
                    nameof(receiptFilter.ReceiptDateFinal)
                ]);

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetReceiptsResponse>(Content);

            receiptResponse?.Results.Should().NotBeNull();
            var results = receiptResponse?.Results.Where(x => x.ReceiptDate >= dateIni && x.ReceiptDate <= dateFinal);
            results.Should().NotBeNull();
        }

        [Fact]
        private async Task OnGivenAValidReceiptItemsGuidsAsReceiptFilter_ShouldBeReturnedAValidReceiptsFromDataBase()
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
                .Build<GetReceiptsRequest>()
                .With(x => x.ReceiptItemIds, new List<Guid> { receiptItemId })
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getReceipts",
                receiptFilter,
                [
                    nameof(receiptFilter.ReceiptItemIds)
                ]);

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetReceiptsResponse>(Content);

            receiptResponse?.Results
                .Should()
                .NotBeNull();

            var foundItems = receiptResponse?
                .Results
                .SelectMany(r => r.ReceiptItems.Where(receiptItem => receiptItem.Id == receiptItemId));

            foundItems.Should().NotBeNull();
        }

        [Fact]
        private async Task OnGivenAValidReceiptItemsNamesAsReceiptFilter_ShouldBeReturnedAValidReceiptsFromDataBase()
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
                .Build<GetReceiptsRequest>()
                .With(x => x.ReceiptItemNames, new List<string> { receiptItemName })
                .Create();

            //Act
            var (StatusCode, Content) = await GetAsync("/getReceipts",
                receiptFilter,
                [
                    nameof(receiptFilter.ReceiptItemNames)
                ]);

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetReceiptsResponse>(Content);

            receiptResponse?.Results
                .Should()
                .NotBeNull();

            var foundItems = receiptResponse?
                .Results
                .SelectMany(r => r.ReceiptItems.Where(receiptItem => receiptItem.ItemName == receiptItemName));

            foundItems.Should().NotBeNull();
        }
    }
}
