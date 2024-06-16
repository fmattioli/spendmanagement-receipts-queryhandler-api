using AutoFixture;

using Contracts.Web.Http.Receipt.Requests;

using FluentAssertions;

using Newtonsoft.Json;
using Receipts.QueryHandler.Application.Queries.Receipt.GetVariableReceipts;
using Receipts.QueryHandler.Domain.Entities;
using Receipts.QueryHandler.Domain.ValueObjects;
using Receipts.QueryHandler.IntegrationTests.Fixtures;
using System.Net;

namespace Receipts.QueryHandler.IntegrationTests.Queries
{
    [Collection(nameof(SharedFixtureCollection))]
    public class ReceiptQueryTests(MongoDBFixture mongoDBFixture, HttpFixture httpFixture)
    {
        private readonly Fixture _fixture = new();
        private readonly MongoDBFixture _mongoDBFixture = mongoDBFixture;
        private readonly HttpFixture _httpFixture = httpFixture;

        [Fact]
        private async Task OnGivenAValidGuidsAsReceiptFilter_ShouldBeReturnedAValidReceipts()
        {
            //Arrange
            var receiptId = _fixture.Create<Guid>();
            var userId = Guid.Parse("c804ff1e-4027-4374-b83a-06151f288536");

            var tenant = _fixture
                .Build<Tenant>()
                .With(x => x.Number, 10000)
                .Create();

            var receipt = _fixture.Build<VariableReceipt>()
                .With(x => x.UserId, userId)
                .With(x => x.Tenant, tenant)
                .With(x => x.Id, receiptId)
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.ReceiptIds, [receiptId])
                .Create();

            //Act
            var (StatusCode, Content) = await _httpFixture.GetAsync("/getVariableReceipts",
                receiptFilter,
                nameof(receiptFilter.ReceiptIds));

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetVariableReceiptsResponse>(Content);

            receiptResponse!.Results.Should().NotBeNull();
            receiptResponse.Results.Should().Contain(x => x.Id.Equals(receiptId));
        }

        [Fact]
        private async Task OnGivenAValidReceiptIdGuidsAsReceiptFilter_ShouldBeReturnedAValidReceipts()
        {
            //Arrange
            var receiptItemId = _fixture.Create<Guid>();
            var userId = Guid.Parse("c804ff1e-4027-4374-b83a-06151f288536");

            var tenant = _fixture
                .Build<Tenant>()
                .With(x => x.Number, 10000)
                .Create();

            var receiptItem = _fixture.Build<ReceiptItem>()
                .With(x => x.Id, receiptItemId)
                .Create();

            var receipt = _fixture.Build<VariableReceipt>()
                .With(x => x.ReceiptItems, [receiptItem])
                .With(x => x.UserId, userId)
                .With(x => x.Tenant, tenant)
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.ReceiptIds, [receipt.Id])
                .With(x => x.ReceiptItemIds, [receiptItemId])
                .Create();

            //Act
            var (StatusCode, Content) = await _httpFixture.GetAsync("/getVariableReceipts",
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
            var category = _fixture.Create<Category>();
            var userId = Guid.Parse("c804ff1e-4027-4374-b83a-06151f288536");

            var tenant = _fixture
                .Build<Tenant>()
                .With(x => x.Number, 10000)
                .Create();

            var receipt = _fixture.Build<VariableReceipt>()
                .With(x => x.Category, category)
                .With(x => x.UserId, userId)
                .With(x => x.Tenant, tenant)
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.CategoryIds, [category.Id])
                .Create();

            //Act
            var (StatusCode, Content) = await _httpFixture.GetAsync("/getVariableReceipts",
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
                .Contain(x => x.Category.Id == category.Id);
        }

        [Fact]
        private async Task OnGivenAValidEstabilishmentNameAsReceiptFilter_ShouldBeReturnedAValidReceipts()
        {
            //Arrange
            var establishmentName = _fixture.Create<string>();
            var userId = Guid.Parse("c804ff1e-4027-4374-b83a-06151f288536");

            var tenant = _fixture
                .Build<Tenant>()
                .With(x => x.Number, 10000)
                .Create();

            var receipt = _fixture.Build<VariableReceipt>()
                .With(x => x.UserId, userId)
                .With(x => x.Tenant, tenant)
                .With(x => x.EstablishmentName, establishmentName)
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.EstablishmentNames, [establishmentName])
                .Create();

            //Act
            var (StatusCode, Content) = await _httpFixture.GetAsync("/getVariableReceipts",
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
            var userId = Guid.Parse("c804ff1e-4027-4374-b83a-06151f288536");

            var tenant = _fixture
                .Build<Tenant>()
                .With(x => x.Number, 10000)
                .Create();

            var receiptOne = _fixture.Build<VariableReceipt>()
                .With(x => x.UserId, userId)
                .With(x => x.Tenant, tenant)
                .With(x => x.ReceiptDate, dateIni)
                .Create();

            var receiptTwo = _fixture.Build<VariableReceipt>()
                .With(x => x.Tenant, tenant)
                .With(x => x.ReceiptDate, dateFinal)
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receiptOne, receiptTwo);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.ReceiptDateInitial, dateIni)
                .With(x => x.ReceiptDateEnd, dateFinal)
                .Create();

            //Act
            var (StatusCode, Content) = await _httpFixture.GetAsync("/getVariableReceipts",
                receiptFilter,
                nameof(receiptFilter.ReceiptDateInitial),
                nameof(receiptFilter.ReceiptDateEnd));

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
            var userId = Guid.Parse("c804ff1e-4027-4374-b83a-06151f288536");

            var receiptItem = _fixture.Build<ReceiptItem>()
                .With(x => x.Id, receiptItemId)
                .Create();

            var tenant = _fixture
                .Build<Tenant>()
                .With(x => x.Number, 10000)
                .Create();

            var receipt = _fixture.Build<VariableReceipt>()
                .With(x => x.UserId, userId)
                .With(x => x.Tenant, tenant)
                .With(x => x.ReceiptItems, [receiptItem])
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.ReceiptItemIds, [receiptItemId])
                .Create();

            //Act
            var (StatusCode, Content) = await _httpFixture.GetAsync("/getVariableReceipts",
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
            var userId = Guid.Parse("c804ff1e-4027-4374-b83a-06151f288536");

            var receiptItem = _fixture.Build<ReceiptItem>()
                .With(x => x.ItemName, receiptItemName)
                .Create();

            var tenant = _fixture
                .Build<Tenant>()
                .With(x => x.Number, 10000)
                .Create();

            var receipt = _fixture.Build<VariableReceipt>()
                .With(x => x.UserId, userId)
                .With(x => x.Tenant, tenant)
                .With(x => x.ReceiptItems, [receiptItem])
                .Create();

            await _mongoDBFixture.InsertReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.ReceiptItemNames, [receiptItemName])
                .Create();

            //Act
            var (StatusCode, Content) = await _httpFixture.GetAsync("/getVariableReceipts",
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
            var userId = Guid.Parse("c804ff1e-4027-4374-b83a-06151f288536");

            var tenant = _fixture
                .Build<Tenant>()
                .With(x => x.Number, 10000)
                .Create();

            var receipt = _fixture.Build<RecurringReceipt>()
                .With(x => x.UserId, userId)
                .With(x => x.Tenant, tenant)
                .Create();

            await _mongoDBFixture.InsertRecurringReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetRecurringReceiptsRequest>()
                .With(x => x.ReceiptIds, [receiptId])
                .Create();

            //Act
            var (StatusCode, Content) = await _httpFixture.GetAsync("/getRecurringReceipts",
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
            var category = _fixture.Create<Category>();
            var userId = Guid.Parse("c804ff1e-4027-4374-b83a-06151f288536");

            var tenant = _fixture
                .Build<Tenant>()
                .With(x => x.Number, 10000)
                .Create();

            var receipt = _fixture.Build<RecurringReceipt>()
                .With(x => x.UserId, userId)
                .With(x => x.Tenant, tenant)
                .With(x => x.Category, category)
                .Create();

            await _mongoDBFixture.InsertRecurringReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.CategoryIds, [category.Id])
                .Create();

            //Act
            var (StatusCode, Content) = await _httpFixture.GetAsync("/getRecurringReceipts",
                receiptFilter,
                nameof(receiptFilter.CategoryIds));

            //Assert
            StatusCode.Should().Be(HttpStatusCode.OK);

            var receiptResponse = JsonConvert.DeserializeObject<GetVariableReceiptsResponse>(Content);

            receiptResponse?.Results.Should().NotBeNull();
            receiptResponse?.Results.Should().Contain(x => x.Category.Id.Equals(category.Id));
        }

        [Fact]
        private async Task OnGivenAValidEstablishmentNameAsRecurringReceiptFilter_ShouldBeReturnedAValidReceipts()
        {
            //Arrange
            var establishmentName = _fixture.Create<string>();
            var userId = Guid.Parse("c804ff1e-4027-4374-b83a-06151f288536");

            var tenant = _fixture
                .Build<Tenant>()
                .With(x => x.Number, 10000)
                .Create();

            var receipt = _fixture.Build<RecurringReceipt>()
                .With(x => x.UserId, userId)
                .With(x => x.Tenant, tenant)
                .With(x => x.EstablishmentName, establishmentName)
                .Create();

            await _mongoDBFixture.InsertRecurringReceiptAsync(receipt);

            var receiptFilter = _fixture
                .Build<GetVariableReceiptsRequest>()
                .With(x => x.EstablishmentNames, [establishmentName])
                .Create();

            //Act
            var (StatusCode, Content) = await _httpFixture.GetAsync("/getRecurringReceipts",
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