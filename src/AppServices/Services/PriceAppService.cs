using Application.Models.Request.Price;
using Application.Models.Response;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Services
{
    public class PriceAppService : IPriceAppService
    {
        private readonly IMapper _mapper;
        private readonly IPriceService _priceService;

        public PriceAppService(IMapper mapper, IPriceService priceService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _priceService = priceService ?? throw new ArgumentNullException(nameof(priceService));
        }

        public Task<long> CreatePrice(CreatePriceRequest createPriceRequest)
        {
            var price = _mapper.Map<Price>(createPriceRequest);

            return _priceService.Create(price);
        }

        public void ExcludePrice(long id)
        {
            _priceService.PriceExists(id);

            _priceService.Delete(id);
        }

        public IEnumerable<PriceResponse> GetAll()
        {
            var priceInfo = _priceService.GetPrices();

            return _mapper.Map<IEnumerable<PriceResponse>>(priceInfo);
        }

        public async Task<UpdatePriceRequest> Get(long id)
        {
            var priceInfo = await _priceService.GetById(id);

            return _mapper.Map<UpdatePriceRequest>(priceInfo);
        }

        public async Task<PriceResponse> GetById(long id)
        {
            var priceInfo = await _priceService.GetById(id);

            return _mapper.Map<PriceResponse>(priceInfo);
        }

        public async Task<PriceResponse> GetPriceByValidity(DateTime departureTime)
        {
            var priceInfo = await _priceService.GetPriceByValidity(departureTime);

            return _mapper.Map<PriceResponse>(priceInfo);
        }

        public void UpdatePrice(long id, UpdatePriceRequest updatePriceRequest)
        {
            var price = _mapper.Map<Price>(updatePriceRequest);

            _priceService.PriceExists(id);

            _priceService.Update(id, price);
        }
    }
}
