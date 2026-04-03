using AutoMapper;
using Pills.Domain.Models;
using Pills.Domain.Models.DTOs.PillTaken;

namespace Pills.Infrastructure.Common.Mapping
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
