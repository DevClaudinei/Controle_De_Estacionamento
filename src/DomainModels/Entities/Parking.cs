using System;
using DomainModels.Interfaces;

namespace DomainModels.Entities
{
    public class Parking : IIdentifable
    {
        protected Parking() { }

        public Parking(
            string plate,
            DateTime arrivalTime,
            DateTime departureTime,
            TimeSpan parkingTime,
            int timeToBeBilled,
            double amountToPay
        )
        {
            Plate = plate;
            ArrivalTime = arrivalTime;
            DepartureTime = departureTime;
            ParkingTime = parkingTime;
            TimeToBeBilled = timeToBeBilled;
            AmountToPay = amountToPay;
        }

        public long Id { get; set; }
        public string Plate { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public TimeSpan ParkingTime { get; set; }
        public int TimeToBeBilled { get; set; }
        public double AmountToPay { get; set; }
        public long PriceId { get; set; }
        public virtual Price Price { get; set; }
    }
}
