using DomainModels.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainServices.Services.Interfaces
{
    public interface IPriceService
    {
        Task<long> Create(Price price);
        IEnumerable<Price> GetPrices();
        Task<Price> GetPriceByValidity(DateTime departureTime);
        void Update(long id, Price price);
        void Delete(long id);
        Task<Price> GetById(long id);
        bool PriceExists(long id);
    }
}
