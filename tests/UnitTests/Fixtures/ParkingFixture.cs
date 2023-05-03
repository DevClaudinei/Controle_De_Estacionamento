using Bogus;
using DomainModels.Entities;
using System.Collections.Generic;

namespace UnitTests.Fixtures
{
    public static class ParkingFixture
    {
        public static Parking ParkingFake()
        {
            var parkingFake = new Faker<Parking>("pt_BR")
                .RuleFor(x => x.Id, f => f.Random.Long(1, 1))
                .RuleFor(x => x.Plate, f => f.Random.String())
                .RuleFor(x => x.ArrivalTime, f => f.Date.Recent())
                .RuleFor(x => x.DepartureTime, f => f.Date.Recent())
                .RuleFor(x => x.ParkingTime, f => f.Date.Timespan())
                .RuleFor(x => x.TimeToBeBilled, f => f.Random.Int())
                .RuleFor(x => x.AmountToPay, f => f.Random.Double())
                .Generate();

            return parkingFake;
        }

        public static IEnumerable<Parking> ParkingFakes(int quantity)
        {
            var parkingFake = new Faker<Parking>("pt_BR")
                .RuleFor(x => x.Plate, f => f.Random.String())
                .RuleFor(x => x.ArrivalTime, f => f.Date.Recent())
                .RuleFor(x => x.DepartureTime, f => f.Date.Recent())
                .RuleFor(x => x.ParkingTime, f => f.Date.Timespan())
                .RuleFor(x => x.TimeToBeBilled, f => f.Random.Int())
                .RuleFor(x => x.AmountToPay, f => f.Random.Double())
                .Generate(quantity);

            return parkingFake;
        }
    }
}
