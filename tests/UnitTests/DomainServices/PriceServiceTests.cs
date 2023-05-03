using Bogus;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services;
using EntityFrameworkCore.QueryBuilder.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Infrastructure.Data.Context;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnitTests.Fixtures;

namespace UnitTests.DomainServices
{
    public class PriceServiceTests
    {
        private readonly PriceService _priceService;       
        private readonly Mock<IUnitOfWork<DataContext>> _mockUnitOfWork;
        private readonly Mock<IRepositoryFactory<DataContext>> _mockRepositoryFactory;

        public PriceServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<DataContext>>();
            _mockRepositoryFactory = new Mock<IRepositoryFactory<DataContext>>();
            _priceService = new(_mockUnitOfWork.Object, _mockRepositoryFactory.Object);
        }

        [Fact]
        public void Should_Execute_Create_Sucessfully()
        {
            // Arrange
            var priceFake = PriceFixture.PriceFake();

            _mockUnitOfWork.Setup(x => x.Repository<Price>().Add(priceFake))
                .Returns(priceFake);

            // Act
            var result = _priceService.Create(priceFake);

            // Assert
            result.Result.Should().Be(priceFake.Id);

            _mockUnitOfWork.Verify(x => x.Repository<Price>().Add(priceFake), Times.Once());
        }

        [Fact]
        public void Should_Return_All_Prices_When_Execute_GetPrices()
        {
            // Arrange
            var pricesFake = PriceFixture.PricesFake(1);

            _mockRepositoryFactory.Setup(x => x.Repository<Price>().MultipleResultQuery())
                .Returns(It.IsAny<IMultipleResultQuery<Price>>());

            _mockRepositoryFactory.Setup(x => x.Repository<Price>()
                .Search(It.IsAny<IMultipleResultQuery<Price>>()))
                .Returns((IList<Price>)pricesFake);

            // Act
            var result = _priceService.GetPrices();

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void Should_Execute_Delete_Sucessfully()
        {
            // Arrange
            var priceFake = PriceFixture.PriceFake();

            _mockRepositoryFactory.Setup(x => x.Repository<Price>().SingleResultQuery()
                .AndFilter(It.IsAny<Expression<Func<Price, bool>>>()))
                .Returns(It.IsAny<IQuery<Price>>());

            _mockRepositoryFactory.Setup(x => x.Repository<Price>()
                .SingleOrDefaultAsync(It.IsAny<IQuery<Price>>(), default))
                .ReturnsAsync(priceFake);

            _mockUnitOfWork.Setup(x => x.Repository<Price>().Remove(It.IsAny<Price>()));

            // Act
            _priceService.Delete(priceFake.Id);

            // Assert
            _mockRepositoryFactory.Verify(x => x.Repository<Price>().SingleResultQuery()
                .AndFilter(It.IsAny<Expression<Func<Price, bool>>>()), Times.Once());

            _mockRepositoryFactory.Verify(x => x.Repository<Price>()
                .SingleOrDefaultAsync(It.IsAny<IQuery<Price>>(), default), Times.Once());

            _mockUnitOfWork.Verify(x => x.Repository<Price>().Remove(It.IsAny<Price>()), Times.Once());
        }

        [Fact]
        public async Task Should_Execute_GetById_Sucessfully()
        {
            // Arrange
            var priceFake = PriceFixture.PriceFake();

            _mockRepositoryFactory.Setup(x => x.Repository<Price>().SingleResultQuery()
                .AndFilter(It.IsAny<Expression<Func<Price, bool>>>()))
                .Returns(It.IsAny<IQuery<Price>>());

            _mockRepositoryFactory.Setup(x => x.Repository<Price>()
                .SingleOrDefaultAsync(It.IsAny<IQuery<Price>>(), default))
                .ReturnsAsync(priceFake);

            // Act
            var result = await _priceService.GetById(priceFake.Id);

            // Assert
            result.Id.Should().Be(result.Id);

            _mockRepositoryFactory.Verify(x => x.Repository<Price>().SingleResultQuery()
                .AndFilter(It.IsAny<Expression<Func<Price, bool>>>()), Times.Once());

            _mockRepositoryFactory.Verify(x => x.Repository<Price>()
                .SingleOrDefaultAsync(It.IsAny<IQuery<Price>>(), default), Times.Once());
        }

        [Fact]
        public async Task Should_Execute_GetPriceByValidity_Sucessfully()
        {
            // Arrange
            var priceFake = PriceFixture.PriceFake();

            _mockRepositoryFactory.Setup(x => x.Repository<Price>().SingleResultQuery()
                .AndFilter(It.IsAny<Expression<Func<Price, bool>>>()))
                .Returns(It.IsAny<IQuery<Price>>());

            _mockRepositoryFactory.Setup(x => x.Repository<Price>()
                .SingleOrDefaultAsync(It.IsAny<IQuery<Price>>(), default))
                .ReturnsAsync(priceFake);

            // Act
            var result = await _priceService.GetPriceByValidity(priceFake.InitialTerm);

            // Assert
            result.InitialTerm.Should().Be(priceFake.InitialTerm);

            _mockRepositoryFactory.Verify(x => x.Repository<Price>().SingleResultQuery()
                .AndFilter(It.IsAny<Expression<Func<Price, bool>>>()), Times.Once());

            _mockRepositoryFactory.Verify(x => x.Repository<Price>()
                .SingleOrDefaultAsync(It.IsAny<IQuery<Price>>(), default), Times.Once());
        }

        [Fact]
        public void Should_Execute_Update_Sucessfully()
        {
            // Arrange
            var priceFake = PriceFixture.PriceFake();

            _mockUnitOfWork.Setup(x => x.Repository<Price>().Update(It.IsAny<Price>()));

            // Act
            _priceService.Update(priceFake.Id, priceFake);

            // Assert
            _mockUnitOfWork.Verify(x => x.Repository<Price>().Update(It.IsAny<Price>()), Times.Once());

            _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
        }

        [Fact]
        public void Should_Pass_When_Execute_PriceExists()
        {
            // Arrange
            var priceFake = PriceFixture.PriceFake();

            _mockRepositoryFactory.Setup(x => x.Repository<Price>().Any(It.IsAny<Expression<Func<Price, bool>>>()))
                .Returns(true);

            // Act
            var result = _priceService.PriceExists(priceFake.Id);

            // Assert
            result.Should().BeTrue();

            _mockRepositoryFactory.Verify(x => x.Repository<Price>().Any(It.IsAny<Expression<Func<Price, bool>>>()), Times.Once());

        }

        [Fact]
        public void Should_Fail_When_Execute_PriceExists()
        {
            // Arrange
            var priceFake = PriceFixture.PriceFake();

            _mockRepositoryFactory.Setup(x => x.Repository<Price>().Any(It.IsAny<Expression<Func<Price, bool>>>()))
                .Returns(false);

            // Act
            Action act = () => _priceService.PriceExists(priceFake.Id);

            // Assert
            act.Should().ThrowExactly<NotFoundException>($"Price not found for Id {priceFake.Id}.");

            _mockRepositoryFactory.Verify(x => x.Repository<Price>().Any(It.IsAny<Expression<Func<Price, bool>>>()), Times.Once());

        }
    }
}
