using AppServices.Profiles;
using AppServices.Services;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using UnitTests.Fixtures;

namespace UnitTests.AppServices
{
    public class PriceAppServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IPriceService> _mockPriceService;
        private readonly PriceAppService _priceAppService;

        public PriceAppServiceTests()
        {
            var config = new MapperConfiguration(opt =>
            {
                opt.AddProfile(new PriceProfile());
            });

            _mapper = config.CreateMapper();
            _mockPriceService = new();
            _priceAppService = new(_mapper, _mockPriceService.Object);
        }

        [Fact]
        public async Task Should_Execute_CreatePrice_Sucessfully()
        {
            // Arrange
            var createPriceRequestFake = CreatePriceRequestFixture.CreatePriceRequestFake();
            var id = 1L;

            _mockPriceService.Setup(x => x.Create(It.IsAny<Price>()))
                .ReturnsAsync(id);

            // Act
            var result = await _priceAppService.CreatePrice(createPriceRequestFake);

            // Assert
            result.Should().Be(id);
        }

        [Fact]
        public void Should_Execute_ExcludePrice_Sucessfully()
        {
            // Arrange
            var id = 1L;

            _mockPriceService.Setup(x => x.PriceExists(It.IsAny<long>()))
                .Returns(true);

            _mockPriceService.Setup(x => x.Delete(It.IsAny<long>()));
            
            // Act
            _priceAppService.ExcludePrice(id);

            // Assert
            _mockPriceService.Verify(x => x.PriceExists(It.IsAny<long>()), Times.Once());

            _mockPriceService.Verify(x => x.Delete(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void Should_Return_All_Prices_When_Execute_GetAll()
        {
            // Arrange
            var pricesFake = PriceFixture.PricesFake(1);

            _mockPriceService.Setup(x => x.GetPrices())
                .Returns(pricesFake);

            // Act
            var result = _priceAppService.GetAll();

            // Assert
            result.Should().HaveCount(1);

            _mockPriceService.Verify(x => x.GetPrices(), Times.Once());
        }

        [Fact]
        public async Task Should_Execute_Get_Sucessfully()
        {
            // Arrange
            var priceFake = PriceFixture.PriceFake();

            _mockPriceService.Setup(x => x.GetById(It.IsAny<long>()))
                .ReturnsAsync(priceFake);

            // Act
            var result = await _priceAppService.Get(priceFake.Id);

            // Assert
            result.Id.Should().Be(priceFake.Id);

            _mockPriceService.Verify(x => x.GetById(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public async Task Should_Execute_GetById_Sucessfully()
        {
            // Arrange
            var priceFake = PriceFixture.PriceFake();

            _mockPriceService.Setup(x => x.GetById(It.IsAny<long>()))
                .ReturnsAsync(priceFake);

            // Act
            var result = await _priceAppService.GetById(priceFake.Id);

            // Assert
            result.Id.Should().Be(priceFake.Id);

            _mockPriceService.Verify(x => x.GetById(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public async Task Should_Execute_GetPriceByValidity_Sucessfully()
        {
            // Arrange
            var priceFake = PriceFixture.PriceFake();

            _mockPriceService.Setup(x => x.GetPriceByValidity(It.IsAny<DateTime>()))
                .ReturnsAsync(priceFake);

            // Act
            var result = await _priceAppService.GetPriceByValidity(priceFake.FinalTerm);

            // Assert
            result.CurrentValue.Should().Be(priceFake.CurrentValue);

            _mockPriceService.Verify(x => x.GetPriceByValidity(It.IsAny<DateTime>()), Times.Once());
        }

        [Fact]
        public void Should_Execute_UpdatePrice_Sucessfully()
        {
            // Arrange
            var updatePriceFake = UpdatePriceRequestFixture.UpdatePriceRequestFake();

            _mockPriceService.Setup(x => x.PriceExists(It.IsAny<long>()))
                .Returns(true);

            _mockPriceService.Setup(x => x.Update(It.IsAny<long>(), It.IsAny<Price>()));

            // Act
            _priceAppService.UpdatePrice(updatePriceFake.Id, updatePriceFake);

            // Assert
            _mockPriceService.Verify(x => x.PriceExists(It.IsAny<long>()), Times.Once());

            _mockPriceService.Verify(x => x.Update(It.IsAny<long>(), It.IsAny<Price>()), Times.Once());
        }
    }
}
