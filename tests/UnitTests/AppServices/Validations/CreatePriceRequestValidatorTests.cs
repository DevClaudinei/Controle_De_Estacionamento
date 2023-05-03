using AppServices.Validations;
using FluentAssertions;
using FluentValidation.TestHelper;
using UnitTests.Fixtures;

namespace UnitTests.AppServices.Validations
{
    public class CreatePriceRequestValidatorTests
    {
        private readonly CreatePriceRequestValidator _createPriceRequestValidator = new();

        [Fact]
        public void Should_Pass_When_Execute_CreatePriceRequestValidator()
        {
            // Arrange
            var createPriceRequestFake = CreatePriceRequestFixture.CreatePriceRequestFake();
            createPriceRequestFake.FinalTerm = createPriceRequestFake.InitialTerm.AddDays(10);

            // Act
            var result = _createPriceRequestValidator.TestValidate(createPriceRequestFake);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Fail_When_Execute_CreatePriceRequestValidator()
        {
            // Arrange
            var createPriceRequestFake = CreatePriceRequestFixture.CreatePriceRequestFake();

            // Act
            var result = _createPriceRequestValidator.TestValidate(createPriceRequestFake);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FinalTerm);
            result.Errors.Should().HaveCount(2);
        }
    }
}
