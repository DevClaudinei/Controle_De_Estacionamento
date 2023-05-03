using AppServices.Profiles;
using AppServices.Services;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using UnitTests.Fixtures;

namespace UnitTests.AppServices
{
    public class ParkingAppServiceTests
    {
        private readonly Mock<IParkingService> _mockParkingService;
        private readonly Mock<IPriceAppService> _mockPriceAppService;
        private readonly ParkingAppService _parkingAppService;
        private readonly IMapper _mapper;

        public ParkingAppServiceTests()
        {
            var config = new MapperConfiguration(opt =>
            {
                opt.AddProfile(new ParkingProfile());
            });

            _mapper = config.CreateMapper();
            _mockParkingService = new ();
            _mockPriceAppService = new ();
            _parkingAppService = new(_mapper, _mockParkingService.Object, _mockPriceAppService.Object);
        }

        [Fact]
        public async Task Should_Pass_When_Execute_CheckInParking()
        {
            // Arrange
            var checkInFake = CheckInRequestFixture.CheckInRequestFake();
            var id = 1L;
            var priceResponseFake = PriceResponseFixture.PriceResponseFake();
            var parkingFake = ParkingFixture.ParkingFake();

            _mockParkingService.Setup(x => x.CheckInParking(It.IsAny<Parking>()))
                .ReturnsAsync(id);

            _mockPriceAppService.Setup(x => x.GetPriceByValidity(It.IsAny<DateTime>()))
                .ReturnsAsync(priceResponseFake);

            _mockParkingService.Setup(x => x.GetByPlate(checkInFake.Plate))
                .ReturnsAsync(parkingFake);

            // Act
            var x = await _parkingAppService.CheckInParking(checkInFake);

            // Assert
            x.Should().Be(id);

            _mockParkingService.Verify(x => x.CheckInParking(It.IsAny<Parking>()), Times.Once());

            _mockPriceAppService.Verify(x => x.GetPriceByValidity(It.IsAny<DateTime>()), Times.Once());

            _mockParkingService.Verify(x => x.GetByPlate(checkInFake.Plate), Times.Once());
        }

        [Fact]
        public async Task Should_Fail_When_Execute_CheckInParking()
        {
            // Arrange
            var checkInFake = CheckInRequestFixture.CheckInRequestFake();
            var priceResponseFake = PriceResponseFixture.PriceResponseFake();
            var parkingFake = ParkingFixture.ParkingFake();
            parkingFake.DepartureTime = new DateTime();


            _mockParkingService.Setup(x => x.CheckInParking(It.IsAny<Parking>()));

            _mockPriceAppService.Setup(x => x.GetPriceByValidity(It.IsAny<DateTime>()))
                .ReturnsAsync(priceResponseFake);

            _mockParkingService.Setup(x => x.GetByPlate(checkInFake.Plate))
                .ReturnsAsync(parkingFake);

            // Act
            var result = async () => await _parkingAppService.CheckInParking(checkInFake);

            // Assert
            await result.Should().ThrowAsync<BadRequestException>()
                .WithMessage($"Vehicle with plate {checkInFake.Plate} is already parked.");

            _mockPriceAppService.Verify(x => x.GetPriceByValidity(It.IsAny<DateTime>()), Times.Once());

            _mockParkingService.Verify(x => x.GetByPlate(checkInFake.Plate), Times.Once());
        }

        [Fact]
        public void Should_Pass_When_Execute_CheckOutParking()
        {
            // Arrange
            var checkOutRequestFake = CheckOutRequestFixture.CheckOutRequestFake();
            var priceResponseFake = PriceResponseFixture.PriceResponseFake();
            var parkingFake = ParkingFixture.ParkingFake();
            parkingFake.DepartureTime = new DateTime();

            _mockParkingService.Setup(x => x.GetById(It.IsAny<long>()))
                .ReturnsAsync(parkingFake);

            _mockPriceAppService.Setup(x => x.GetById(It.IsAny<long>()))
                .ReturnsAsync(priceResponseFake);

            _mockParkingService.Setup(x => x.CheckOutParking(It.IsAny<long>(), It.IsAny<Parking>()));

            // Act
            _parkingAppService.CheckOutParking(checkOutRequestFake.Id, checkOutRequestFake);

            _mockParkingService.Verify(x => x.GetById(It.IsAny<long>()), Times.Once());

            _mockPriceAppService.Verify(x => x.GetById(It.IsAny<long>()), Times.Once());

            _mockParkingService.Verify(x => x.CheckOutParking(It.IsAny<long>(), It.IsAny<Parking>()), Times.Once());
        }

        [Fact]
        public void Should_Fail_When_Execute_CheckOutParking()
        {
            // Arrange
            var checkOutRequestFake = CheckOutRequestFixture.CheckOutRequestFake();
            var priceResponseFake = PriceResponseFixture.PriceResponseFake();
            var parkingFake = ParkingFixture.ParkingFake();

            _mockParkingService.Setup(x => x.GetById(It.IsAny<long>()))
                .ReturnsAsync(parkingFake);

            _mockPriceAppService.Setup(x => x.GetById(It.IsAny<long>()))
                .ReturnsAsync(priceResponseFake);

            _mockParkingService.Setup(x => x.CheckOutParking(It.IsAny<long>(), It.IsAny<Parking>()));

            // Act
            Action act = () => _parkingAppService.CheckOutParking(checkOutRequestFake.Id, checkOutRequestFake);

            act.Should().ThrowExactly<BadRequestException>("Unable to perform operations on this record.");

            _mockParkingService.Verify(x => x.GetById(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void Should_Execute_ExcludeParkingInfo_Sucessfully()
        {
            // Arrange
            var id = 1L;

            _mockParkingService.Setup(x => x.ExcludeParkingInfo(It.IsAny<long>()));

            // Act
            _parkingAppService.ExcludeParkingInfo(id);

            // Assert
            _mockParkingService.Verify(x => x.ExcludeParkingInfo(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void Should_Return_All_Parkings_When_Execute_GetAll()
        {
            // Arrange
            var parkingsFake = ParkingFixture.ParkingFakes(1);

            _mockParkingService.Setup(x => x.GetAll())
                .Returns(parkingsFake);

            // Act
            var result = _parkingAppService.GetAll();

            // Assert
            result.Should().HaveCount(1);

            _mockParkingService.Verify(x => x.GetAll(), Times.Once());
        }

        [Fact]
        public async Task Should_Pass_When_Execute_GetById()
        {
            // Arrange
            var parkingFake = ParkingFixture.ParkingFake();

            _mockParkingService.Setup(x => x.GetById(It.IsAny<long>()))
                .ReturnsAsync(parkingFake);

            // Act
            var result = await _parkingAppService.GetById(parkingFake.Id);

            // Assert
            result.Id.Should().Be(parkingFake.Id);
        }

        [Fact]
        public async Task Should_Fail_When_Execute_GetById()
        {
            // Arrange
            var parkingFake = ParkingFixture.ParkingFake();

            _mockParkingService.Setup(x => x.GetById(It.IsAny<long>()));

            // Act
            var result = async () => await _parkingAppService.GetById(parkingFake.Id);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>()
    .           WithMessage($"Parking info for Id: {parkingFake.Id} not found.");

            _mockParkingService.Verify(x => x.GetById(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public async Task Should_Pass_When_Execute_Get()
        {
            // Arrange
            var parkingFake = ParkingFixture.ParkingFake();

            _mockParkingService.Setup(x => x.GetById(It.IsAny<long>()))
                .ReturnsAsync(parkingFake);

            // Act
            var result = await _parkingAppService.Get(parkingFake.Id);

            // Assert
            result.Id.Should().Be(parkingFake.Id);

            _mockParkingService.Verify(x => x.GetById(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public async Task Should_Fail_When_Execute_Get()
        {
            // Arrange
            var parkingFake = ParkingFixture.ParkingFake();

            _mockParkingService.Setup(x => x.GetById(It.IsAny<long>()));

            // Act
            var result = async () => await _parkingAppService.Get(parkingFake.Id);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Parking info for Id: {parkingFake.Id} not found.");

            _mockParkingService.Verify(x => x.GetById(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public async Task Should_Pass_When_Execute_GetByPlate()
        {
            // Arrange
            var parkingFake = ParkingFixture.ParkingFake();

            _mockParkingService.Setup(x => x.GetByPlate(It.IsAny<string>()))
                .ReturnsAsync(parkingFake);

            // Act
            var result = await _parkingAppService.GetByPlate(parkingFake.Plate);

            // Assert
            result.Plate.Should().Be(parkingFake.Plate);
        }

        [Fact]
        public async Task Should_Fail_When_Execute_GetByPlate()
        {
            // Arrange
            var parkingFake = ParkingFixture.ParkingFake();

            _mockParkingService.Setup(x => x.GetByPlate(It.IsAny<string>()));

            // Act
            var result = async () => await _parkingAppService.GetByPlate(parkingFake.Plate);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Parking info for Plate: {parkingFake.Plate} not found.");
        }
    }
}
