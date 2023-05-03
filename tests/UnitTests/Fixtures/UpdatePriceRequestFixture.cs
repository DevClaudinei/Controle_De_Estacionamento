using Application.Models.Request.Price;
using Bogus;

namespace UnitTests.Fixtures
{
    public static class UpdatePriceRequestFixture
    {
        public static UpdatePriceRequest UpdatePriceRequestFake()
        {
            var updatePriceRequestFake = new Faker<UpdatePriceRequest>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long())
                .RuleFor(x => x.InitialTerm, f => f.Date.Recent())
                .RuleFor(x => x.InitialTerm, f => f.Date.Recent())
                .RuleFor(x => x.CurrentValue, f => f.Random.Double())
                .Generate();

            return updatePriceRequestFake;
        }
    }
}
