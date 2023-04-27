using System;

namespace Application.Models.Request.Price
{
    public class CreatePriceRequest
    {
        public DateTime InitialTerm { get; set; }
        public DateTime FinalTerm { get; set; }
        public double CurrentValue { get; set; }
    }
}
