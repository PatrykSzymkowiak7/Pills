using AutoMapper;
using Pills.Domain.Models;
using Pills.Application.DTOs.PillTaken;

namespace Pills.Web.Mapping
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
