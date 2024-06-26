using FluentAssertions;
using NetArchTest.Rules;

namespace Receipts.QueryHandler.UnitTests.Arch
{

    public class ArchTests
    {
        [Fact]
        public void ApplicationServicesLayer_Should_Not_HaveDependencyOnOthers()
        {
            //Arrange
            var assembly = typeof(Application.Mappers.CategoryMapper).Assembly;

            //Act
            var result = Types.InAssembly(assembly)
                .Should()
                .NotHaveDependencyOn("Receipts.QueryHandler.Api")
                .And()
                .NotHaveDependencyOn("Receipts.QueryHandler.CrossCutting")
                .And()
                .NotHaveDependencyOn("Receipts.QueryHandler.Data")
                .GetResult();

            //Assert
            result.IsSuccessful
                .Should()
                .BeTrue();
        }


        [Fact]
        public void CrossCuttingLayer_Should_Not_HaveDependencyOnOthers()
        {
            //Arrange
            var assembly = typeof(CrossCutting.Config.Credentials).Assembly;

            //Act
            var result = Types.InAssembly(assembly)
                .Should()
                .NotHaveDependencyOn("Receipts.QueryHandler.Api")
                .GetResult();

            //Assert
            result.IsSuccessful
                .Should()
                .BeTrue();
        }

        [Fact]
        public void DomainLayer_Should_Not_HaveDependencyOnOthers()
        {
            //Arrange
            var assembly = typeof(Domain.Entities.Category).Assembly;

            //Act
            var result = Types.InAssembly(assembly)
                .Should()
                .NotHaveDependencyOn("Receipts.QueryHandler.Application")
                .And()
                .NotHaveDependencyOn("Receipts.QueryHandler.Data")
                .And()
                .NotHaveDependencyOn("Receipts.QueryHandler.Api")
                .And()
                .NotHaveDependencyOn("Receipts.QueryHandler.CrossCutting")
                .GetResult();

            //Assert
            result.IsSuccessful
                .Should()
                .BeTrue();
        }

        [Fact]
        public void DataLayer_Should_Not_HaveDependencyOnOthers()
        {
            //Arrange
            var assembly = typeof(Data.Constants.DataConstants).Assembly;

            //Act
            var result = Types.InAssembly(assembly)
                .Should()
                .NotHaveDependencyOn("Receipts.QueryHandler.Application")
                .And()
                .NotHaveDependencyOn("Receipts.QueryHandler.Api")
                .And()
                .NotHaveDependencyOn("Receipts.QueryHandler.CrossCutting")
                .GetResult();

            //Assert
            result.IsSuccessful
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Api_Should_Not_HaveDependencyOnDomainAndData()
        {
            //Arrange
            var assembly = typeof(Api.Controllers.CategoryController).Assembly;

            //Act
            var result = Types.InAssembly(assembly)
                .Should()
                .NotHaveDependencyOn("Receipts.QueryHandler.Data")
                .And()
                .NotHaveDependencyOn("Receipts.QueryHandler.Domain")
                .GetResult();

            //Assert
            result.IsSuccessful
                .Should()
                .BeTrue();
        }
    }

}
