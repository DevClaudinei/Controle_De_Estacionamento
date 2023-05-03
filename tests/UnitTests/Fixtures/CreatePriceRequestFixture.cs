using Application.Models.Request.Price;
using Bogus;

namespace UnitTests.Fixtures
{
    public static class CreatePriceRequestFixture
    {
        public static CreatePriceRequest CreatePriceRequestFake()
        {
            var createPriceRequestFake = new Faker<CreatePriceRequest>("pt_BR")
                .RuleFor(x => x.InitialTerm, f => f.Date.Recent())
                .RuleFor(x => x.InitialTerm, f => f.Date.Recent())
                .RuleFor(x => x.CurrentValue, f => f.Random.Double())
                .Generate();

            return createPriceRequestFake;
        }
    }
}
