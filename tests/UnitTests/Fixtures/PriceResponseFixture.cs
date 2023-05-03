using Application.Models.Response;
using Bogus;

namespace UnitTests.Fixtures
{
    public static class PriceResponseFixture
    {
        public static PriceResponse PriceResponseFake()
        {
            var priceResponseFake = new Faker<PriceResponse>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1))
                .RuleFor(x => x.InitialTerm, f => f.Date.Recent())
                .RuleFor(x => x.InitialTerm, f => f.Date.Recent())
                .RuleFor(x => x.CurrentValue, f => f.Random.Double())
                .Generate();

            return priceResponseFake;
        }
    }
}
