using Application.Models.Request.Parking;
using Application.Models.Response;
using AppServices.Services.Interfaces;
using AutoMapper;
using DomainModels.Entities;
using DomainServices.Exceptions;
using DomainServices.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppServices.Services
{
    public class ParkingAppService : IParkingAppService
    {
        private readonly IParkingService _parkingService;
        private readonly IPriceAppService _priceAppService;
        private readonly IMapper _mapper;

        public ParkingAppService(IMapper mapper, IParkingService parkingService, IPriceAppService priceAppService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _parkingService = parkingService ?? throw new ArgumentNullException(nameof(parkingService));
            _priceAppService = priceAppService ?? throw new ArgumentNullException(nameof(priceAppService));
        }

        public async Task<long> CheckInParking(CheckInRequest checkInRequest)
        {
            var parking = _mapper.Map<Parking>(checkInRequest);

            var price = _priceAppService.GetPriceByValidity(checkInRequest.ArrivalTime).Result;
            VehicleCanPark(checkInRequest.Plate);
            parking.PriceId = price.Id;
            
            return await _parkingService.CheckInParking(parking);
        }

        private bool VehicleCanPark(string plate)
        {
            var vehicleFound = _parkingService.GetByPlate(plate).Result;
            var dateNull = new DateTime();

            if (vehicleFound != null && vehicleFound.DepartureTime == dateNull)
                throw new BadRequestException($"Vehicle with plate {plate} is already parked.");

            return true;
        }

        public void CheckOutParking(long id, CheckOutRequest checkOutRequest)
        {
            var dateNull = new DateTime();
            var parkingFound = GetById(id);

            if (parkingFound.Result.DepartureTime != dateNull)
                throw new BadRequestException("Não é Possivel Realizar operações nesse registro.");

            var price = _priceAppService.GetById(checkOutRequest.PriceId).Result;
            var parking = _mapper.Map<Parking>(checkOutRequest);

            PreparingParkingProperties(parking, price);

            _parkingService.CheckOutParking(id, parking);
        }

        private void PreparingParkingProperties(Parking parking, PriceResponse price)
        {
            parking.ParkingTime = parking.DepartureTime - parking.ArrivalTime;
            var days = parking.ParkingTime.Days * 24;
            var valorDasHoras = CalculateParkingTime(parking.ParkingTime);
            parking.TimeToBeBilled = days + valorDasHoras;

            if (days >= 1)
                parking.AmountToPay += (10 * price.CurrentValue);

            if (valorDasHoras >= 10)
                parking.AmountToPay += (10 * price.CurrentValue);

            if (days < 1 && valorDasHoras < 10)
                parking.AmountToPay = parking.TimeToBeBilled * price.CurrentValue;
        }

        private int CalculateParkingTime(TimeSpan duracao)
        {
            var hora = duracao.Hours;
            var minutos = duracao.Minutes;

            if (hora == 0 && minutos <= 30)
                return 1;
            if (hora == 0 && minutos > 30 || hora > 0 && minutos <= 10)
                return hora;

            return hora + 1;
        }

        public void ExcludeParkingInfo(long id)
        {
            _parkingService.ExcludeParkingInfo(id);
        }

        public IEnumerable<ParkingResponse> GetAll()
        {
            var parkingsInfos = _parkingService.GetAll();

            return _mapper.Map<IEnumerable<ParkingResponse>>(parkingsInfos);
        }

        public async Task<CheckOutRequest> GetById(long id)
        {
            var parkingInfo = await _parkingService.GetByIdId(id)
                ?? throw new NotFoundException($"Parking info for Id: {id} not found.");

            return _mapper.Map<CheckOutRequest>(parkingInfo);
        }

        public async Task<ParkingResponse> Get(long id)
        {
            var parkingInfo = await _parkingService.GetByIdId(id)
                ?? throw new NotFoundException($"Parking info for Id: {id} not found.");

            return _mapper.Map<ParkingResponse>(parkingInfo);
        }

        public async Task<ParkingResponse> GetByPlate(string plate)
        {
            var parkingInfo = await _parkingService.GetByPlate(plate)
                ?? throw new NotFoundException($"Parking info for Plate: {plate} not found.");

            return _mapper.Map<ParkingResponse>(parkingInfo);
        }
    }
}
