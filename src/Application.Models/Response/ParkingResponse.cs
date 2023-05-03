using System;

namespace Application.Models.Response
{
    public class ParkingResponse
    {
        public long Id { get; set; }
        public string Plate { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public TimeSpan ParkingTime { get; set; }
        public int TimeToBeBilled { get; set; }
        public double Price { get; set; }
        public double AmountToPay { get; set; }
    }
}
