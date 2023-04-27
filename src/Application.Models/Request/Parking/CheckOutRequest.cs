using System;

namespace Application.Models.Request.Parking
{
    public class CheckOutRequest
    {
        public long Id { get; set; }
        public string Plate { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public TimeSpan ParkingTime { get; set; }
        public int TimeToBeBilled { get; set; }
        public long PriceId { get; set; }
        public double AmountToPay { get; set; }
    }
}
