using AutoMapper;
using Pills.Models;
using Pills.Models.DTOs.PillTypes;
using Pills.Models.ViewModels.PillTypes;

namespace Pills.Common.Mapping
{
    public class PillsTypeProfile : Profile
    {
        public PillsTypeProfile()
        {
            CreateMap<PillsTypes, PillTypeDto>()
                .ForMember(dest => dest.MultiplePerDayAllowed,
                    opt => opt.MapFrom(src => src.MaxAllowed > 1));

            CreateMap<CreatePillTypeDto, PillsTypes>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name.Trim()));

            CreateMap<CreatePillTypeViewModel, CreatePillTypeDto>();

            CreateMap<EditPillTypeViewModel, EditPillTypeDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name.Trim()));

            CreateMap<EditPillTypeDto, PillsTypes>();
        }
    }
}
