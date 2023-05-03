using Application.Models.Request.Parking;
using Application.Models.Response;
using AutoMapper;
using DomainModels.Entities;

namespace AppServices.Profiles
{
    public class ParkingProfile : Profile
    {
        public ParkingProfile()
        {
            CreateMap<CheckInRequest, Parking>();
            CreateMap<CheckOutRequest, Parking>();
            CreateMap<Parking, ParkingResponse>()
                .ForMember(x => x.Plate, opts => opts.MapFrom(x => x.Plate))
                .ForMember(x => x.ArrivalTime, opts => opts.MapFrom(x => x.ArrivalTime))
                .ForMember(x => x.DepartureTime, opts => opts.MapFrom(x => x.DepartureTime))
                .ForMember(x => x.ParkingTime, opts => opts.MapFrom(x => x.ParkingTime))
                .ForMember(x => x.TimeToBeBilled, opts => opts.MapFrom(x => x.TimeToBeBilled))
                .ForMember(x => x.Price, opts => opts.MapFrom(x => x.Price))
                .ForMember(x => x.AmountToPay, opts => opts.MapFrom(x => x.AmountToPay));

            CreateMap<Parking, CheckOutRequest>()
                .ForMember(x => x.Plate, opts => opts.MapFrom(x => x.Plate))
                .ForMember(x => x.ArrivalTime, opts => opts.MapFrom(x => x.ArrivalTime))
                .ForMember(x => x.DepartureTime, opts => opts.MapFrom(x => x.DepartureTime));
        }
    }
}
