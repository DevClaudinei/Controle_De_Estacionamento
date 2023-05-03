using System;

namespace Application.Models.Response
{
    public class PriceResponse
    {
        public long Id { get; set; }
        public DateTime InitialTerm { get; set; }
        public DateTime FinalTerm { get; set; }
        public double CurrentValue { get; set; }
    }
}
