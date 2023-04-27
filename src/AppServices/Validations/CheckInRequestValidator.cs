using Application.Models.Request.Parking;
using FluentValidation;

namespace AppServices.Validations
{
    public class CheckInRequestValidator : AbstractValidator<CheckInRequest>
    {
        public CheckInRequestValidator() 
        {
            RuleFor(x => x.Plate)
                .NotEmpty()
                .NotNull()
                .Length(8)
                .Must(x => x.LicensePlateIsValid())
                .WithMessage("License plate must be in the format ABC-1234 or ABC-7A45");
        }
    }
}
