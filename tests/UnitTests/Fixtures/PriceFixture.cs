using Bogus;
using DomainModels.Entities;
using System.Collections.Generic;

namespace UnitTests.Fixtures
{
    public static class PriceFixture
    {
        public static Price PriceFake()
        {
            var priceFixture = new Faker<Price>("pt_BR")
                .CustomInstantiator(f => new Price(
                    initialTerm: f.Date.Recent(),
                    finalTerm: f.Date.Recent(),
                    currentValue: f.Random.Double(),
                    parkings: ParkingFixture.ParkingFakes(1)
                    ))
                .Generate();

            return priceFixture;
        }

        public static IEnumerable<Price> PricesFake(int quantity)
        {
            var pricesFake = new Faker<Price>("pt_BR")
                .CustomInstantiator(f => new Price(
                    initialTerm: f.Date.Recent(),
                    finalTerm: f.Date.Recent(),
                    currentValue: f.Random.Double(),
                    parkings: ParkingFixture.ParkingFakes(1)
                    ))
                .Generate(quantity);

            return pricesFake;
        }
    }
}
