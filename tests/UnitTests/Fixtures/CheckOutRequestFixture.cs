using Application.Models.Request.Parking;
using Bogus;

namespace UnitTests.Fixtures
{
    public static class CheckOutRequestFixture
    {
        public static CheckOutRequest CheckOutRequestFake()
        {
            var checkOutRequestFixture = new Faker<CheckOutRequest>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long())
                .RuleFor(x => x.Plate, f => f.Random.String())
                .RuleFor(x => x.ArrivalTime, f => f.Date.Recent())
                .RuleFor(x => x.DepartureTime, f => f.Date.Recent())
                .RuleFor(x => x.ParkingTime, f => f.Date.Timespan())
                .RuleFor(x => x.TimeToBeBilled, f => f.Random.Int())
                .RuleFor(x => x.PriceId, f => f.Random.Int())
                .RuleFor(x => x.AmountToPay, f => f.Random.Double())
                .Generate();

            return checkOutRequestFixture;
        }
    }
}
