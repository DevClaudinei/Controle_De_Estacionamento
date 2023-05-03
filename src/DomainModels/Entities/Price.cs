using System;
using System.Collections.Generic;
using DomainModels.Interfaces;

namespace DomainModels.Entities
{
    public class Price : IIdentifable
    {
        protected Price() { }

        public Price(
            DateTime initialTerm,
            DateTime finalTerm,
            double currentValue,
            IEnumerable<Parking> parkings
        )
        {
            InitialTerm = initialTerm;
            FinalTerm = finalTerm;
            CurrentValue = currentValue;
            Parkings = parkings;
        }

        public long Id { get; set; }
        public DateTime InitialTerm { get; set; }
        public DateTime FinalTerm { get; set; }
        public double CurrentValue { get; set; }
        public IEnumerable<Parking> Parkings { get; set; }
    }
}
