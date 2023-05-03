using Application.Models.Request.Parking;
using Bogus;

namespace UnitTests.Fixtures
{
    public static class CheckInRequestFixture
    {
        public static CheckInRequest CheckInRequestFake()
        {
            var checkInRequestFixture = new Faker<CheckInRequest>("pt_BR")
                .CustomInstantiator(f => new CheckInRequest(
                        plate: f.Random.String(),
                        arrivalTime: f.Date.Recent()
                    ))
                    .Generate();

            return checkInRequestFixture;
        }
    }
}
