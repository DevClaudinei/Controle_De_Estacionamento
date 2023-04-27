using Application.Models.Request.Price;
using Application.Models.Response;
using AutoMapper;
using DomainModels.Entities;

namespace AppServices.Profiles
{
    public class PriceProfile : Profile
    {
        public PriceProfile()
        {
            CreateMap<CreatePriceRequest, Price>();
            CreateMap<UpdatePriceRequest, Price>().ReverseMap();
            CreateMap<Price, PriceResponse>()
                .ForMember(x => x.InitialTerm, opts => opts.MapFrom(x => x.InitialTerm))
                .ForMember(x => x.FinalTerm, opts => opts.MapFrom(x => x.FinalTerm))
                .ForMember(x => x.CurrentValue, opts => opts.MapFrom(x => x.CurrentValue));
        }
    }
}
