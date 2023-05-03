using AppServices.Validations;
using FluentValidation.TestHelper;
using UnitTests.Fixtures;

namespace UnitTests.AppServices.Validations
{
    public class CheckInRequestValidatorTests
    {
        private readonly CheckInRequestValidator _checkInRequestValidator = new();

        [Fact]
        public void Should_Pass_When_Execute_CheckInRequestValidator()
        {
            // Arrange
            var checkInRequestFake = CheckInRequestFixture.CheckInRequestFake();
            checkInRequestFake.Plate = "ABC-1J23";

            // Act
            var result = _checkInRequestValidator.TestValidate(checkInRequestFake);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Fail_When_Execute_CheckInRequestValidator()
        {
            // Arrange
            var checkInRequestFake = CheckInRequestFixture.CheckInRequestFake();

            // Act
            var result = _checkInRequestValidator.TestValidate(checkInRequestFake);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Plate);
        }
    }
}
