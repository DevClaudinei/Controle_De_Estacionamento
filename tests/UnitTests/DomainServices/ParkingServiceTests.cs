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
using UnitTests.Fixtures;

namespace UnitTests.DomainServices
{
    public class ParkingServiceTests
    {
        private readonly ParkingService _parkingService;
        private readonly Mock<IUnitOfWork<DataContext>> _mockUnitOfWork;
        private readonly Mock<IRepositoryFactory<DataContext>> _mockRepositoryFactory;

        public ParkingServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<DataContext>>();
            _mockRepositoryFactory = new Mock<IRepositoryFactory<DataContext>>();
            _parkingService = new(_mockUnitOfWork.Object, _mockRepositoryFactory.Object);
        }

        [Fact]
        public void Should_Execute_CheckInParking_Sucessfully()
        {
            // Arrange
            var parkingFake = ParkingFixture.ParkingFake();

            _mockUnitOfWork.Setup(x => x.Repository<Parking>().Add(parkingFake))
                .Returns(parkingFake);

            // Act
            var result = _parkingService.CheckInParking(parkingFake);

            // Assert
            result.Result.Should().Be(parkingFake.Id);

            _mockUnitOfWork.Verify(x => x.Repository<Parking>().Add(parkingFake), Times.Once());
        }

        [Fact]
        public void Should_Execute_GetAll_Sucessfully()
        {
            // Arrange
            var parkingsFake = ParkingFixture.ParkingFakes(1);

            _mockRepositoryFactory.Setup(x => x.Repository<Parking>().MultipleResultQuery())
                .Returns(It.IsAny<IMultipleResultQuery<Parking>>());

            _mockRepositoryFactory.Setup(x => x.Repository<Parking>()
                .Search(It.IsAny<IMultipleResultQuery<Parking>>()))
                .Returns((IList<Parking>)parkingsFake);

            // Act
            var result = _parkingService.GetAll();

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void Should_Execute_GetById_Sucessfully()
        {
            // Arrange
            var parkingFake = ParkingFixture.ParkingFake();

            _mockRepositoryFactory.Setup(x => x.Repository<Parking>().SingleResultQuery()
                .AndFilter(It.IsAny<Expression<Func<Parking, bool>>>()))
                .Returns(It.IsAny<IQuery<Parking>>());

            _mockRepositoryFactory.Setup(x => x.Repository<Parking>()
                .SingleOrDefaultAsync(It.IsAny<IQuery<Parking>>(), default))
                .ReturnsAsync(parkingFake);

            // Act
            var result = _parkingService.GetById(parkingFake.Id);

            // Assert
            result.Result.Id.Should().Be(parkingFake.Id);
        }

        [Fact]
        public void Should_Execute_GetByPlate_Sucessfully()
        {
            // Arrange
            var parkingFake = ParkingFixture.ParkingFake();

            _mockRepositoryFactory.Setup(x => x.Repository<Parking>().SingleResultQuery()
                .AndFilter(It.IsAny<Expression<Func<Parking, bool>>>()))
                .Returns(It.IsAny<IQuery<Parking>>());

            _mockRepositoryFactory.Setup(x => x.Repository<Parking>()
                .SingleOrDefaultAsync(It.IsAny<IQuery<Parking>>(), default))
                .ReturnsAsync(parkingFake);

            // Act
            var result = _parkingService.GetByPlate(parkingFake.Plate);

            // Assert
            result.Result.Plate.Should().Be(parkingFake.Plate);
        }

        [Fact]
        public void Should_Pass_When_Execute_CheckOutParking()
        {
            // Arrange
            var parkingFake = ParkingFixture.ParkingFake();

            _mockRepositoryFactory.Setup(x => x.Repository<Parking>()
                .Any(It.IsAny<Expression<Func<Parking, bool>>>())).Returns(true);

            _mockUnitOfWork.Setup(x => x.Repository<Parking>().Update(It.IsAny<Parking>()));

            // Act
            _parkingService.CheckOutParking(parkingFake.Id, parkingFake);

            // Assert
            _mockRepositoryFactory.Verify(x => x.Repository<Parking>(), Times.Once());

            _mockUnitOfWork.Setup(x => x.Repository<Parking>().Update(It.IsAny<Parking>()));
            _mockUnitOfWork.Verify(x => x.Repository<Parking>().Update(It.IsAny<Parking>()), Times.Once());

            _mockUnitOfWork.Verify(x => x.SaveChanges(true, false), Times.Once());
        }

        [Fact]
        public void Should_Fail_When_Execute_CheckOutParking()
        {
            // Arrange
            var parkingFake = ParkingFixture.ParkingFake();

            _mockRepositoryFactory.Setup(x => x.Repository<Parking>()
                .Any(It.IsAny<Expression<Func<Parking, bool>>>())).Returns(false);

            _mockUnitOfWork.Setup(x => x.Repository<Parking>().Update(It.IsAny<Parking>()));

            // Act
            Action act = () => _parkingService.CheckOutParking(parkingFake.Id, parkingFake);

            // Assert
            act.Should().ThrowExactly<NotFoundException>($"Parking not found for Id {parkingFake.Id}.");

            _mockRepositoryFactory.Verify(x => x.Repository<Parking>(), Times.Once());

            _mockUnitOfWork.Verify(x => x.Repository<Parking>().Update(It.IsAny<Parking>()), Times.Never());
        }

        [Fact]
        public void Should_Pass_When_Execute_ExcludeParkingInfo()
        {
            // Arrange
            var parkingFake = ParkingFixture.ParkingFake();

            _mockRepositoryFactory.Setup(x => x.Repository<Parking>().SingleResultQuery()
                .AndFilter(It.IsAny<Expression<Func<Parking, bool>>>()))
                .Returns(It.IsAny<IQuery<Parking>>());

            _mockRepositoryFactory.Setup(x => x.Repository<Parking>()
                .SingleOrDefaultAsync(It.IsAny<IQuery<Parking>>(), default))
                .ReturnsAsync(parkingFake);

            _mockUnitOfWork.Setup(x => x.Repository<Parking>().Remove(It.IsAny<Parking>()));

            // Act
            _parkingService.ExcludeParkingInfo(parkingFake.Id);

            // Assert
            _mockRepositoryFactory.Verify(x => x.Repository<Parking>().SingleResultQuery()
                .AndFilter(It.IsAny<Expression<Func<Parking, bool>>>()), Times.Once());

            _mockRepositoryFactory.Verify(x => x.Repository<Parking>()
                .SingleOrDefaultAsync(It.IsAny<IQuery<Parking>>(), default), Times.Once());

            _mockUnitOfWork.Verify(x => x.Repository<Parking>().Remove(It.IsAny<Parking>()), Times.Once());

        }

        [Fact]
        public void Should_Fail_When_Execute_ExcludeParkingInfo()
        {
            // Arrange
            var parkingFake = ParkingFixture.ParkingFake();

            _mockRepositoryFactory.Setup(x => x.Repository<Parking>().SingleResultQuery()
                .AndFilter(It.IsAny<Expression<Func<Parking, bool>>>()))
                .Returns(It.IsAny<IQuery<Parking>>());

            _mockRepositoryFactory.Setup(x => x.Repository<Parking>()
                .SingleOrDefaultAsync(It.IsAny<IQuery<Parking>>(), default));

            _mockUnitOfWork.Setup(x => x.Repository<Parking>().Remove(It.IsAny<Parking>()));

            // Act
            Action act = () => _parkingService.ExcludeParkingInfo(parkingFake.Id);

            // Assert
            act.Should().Throw<NotFoundException>($"Parking info for Id: {parkingFake.Id} not found.");

            _mockRepositoryFactory.Verify(x => x.Repository<Parking>().SingleResultQuery()
                .AndFilter(It.IsAny<Expression<Func<Parking, bool>>>()), Times.Once());

            _mockRepositoryFactory.Verify(x => x.Repository<Parking>()
                .SingleOrDefaultAsync(It.IsAny<IQuery<Parking>>(), default), Times.Once());

            _mockUnitOfWork.Verify(x => x.Repository<Parking>().Remove(It.IsAny<Parking>()), Times.Never());
        }
    }
}
