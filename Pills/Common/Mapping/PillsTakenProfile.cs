using AutoMapper;
using Pills.Models;
using Pills.Models.DTOs.PillTaken;

namespace Pills.Common.Mapping
{
    public class PillsTakenProfile : Profile
    {
        public PillsTakenProfile()
        {
            CreateMap<PillsTaken, TakePillDto>();
            CreateMap<TakePillDto, PillsTaken>();

            CreateMap<PillsTaken, PillTakenDto>();
            CreateMap<PillTakenDto, PillsTaken>();
        }
    }
}
