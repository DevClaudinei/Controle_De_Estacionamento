using System;

namespace Application.Models.Request.Parking
{
    public class CheckInRequest
    {
        public CheckInRequest() { }

        public CheckInRequest(string plate, DateTime arrivalTime)
        {
            Plate = plate.Trim();
            ArrivalTime = arrivalTime;
        }

        public string Plate { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
