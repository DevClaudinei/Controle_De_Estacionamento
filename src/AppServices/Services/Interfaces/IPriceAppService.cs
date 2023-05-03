using Application.Models.Request.Price;
using Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Services.Interfaces
{
    public interface IPriceAppService
    {
        Task<long> CreatePrice(CreatePriceRequest createPriceRequest);
        IEnumerable<PriceResponse> GetAll();
        Task<PriceResponse> GetById(long id);
        Task<UpdatePriceRequest> Get(long id);
        Task<PriceResponse> GetPriceByValidity(DateTime departureTime);
        void UpdatePrice(long id, UpdatePriceRequest updatePriceRequest);
        void ExcludePrice(long id);
    }
}
