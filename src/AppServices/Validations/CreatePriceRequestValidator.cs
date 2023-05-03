using Application.Models.Request.Price;
using FluentValidation;

namespace AppServices.Validations
{
    public class CreatePriceRequestValidator : AbstractValidator<CreatePriceRequest>
    {
        public CreatePriceRequestValidator()
        {
            RuleFor(x => x.InitialTerm)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.FinalTerm)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x)
                .Must(x => x.FinalTerm > x.InitialTerm)
                .WithMessage("Final term cannot be less than initial Term");
        }
    }
}
