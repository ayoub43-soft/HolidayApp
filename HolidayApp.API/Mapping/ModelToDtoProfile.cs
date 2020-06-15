using AutoMapper;
using HolidayApp.API.Dtos;
using HolidayApp.API.Models;

namespace HolidayApp.API.Mapping
{
    public class ModelToDtoProfile : Profile
    {
        public ModelToDtoProfile()
        {
            CreateMap<User, UserForListDto>();
            CreateMap<User, UserForDetailedDto>();
        }
    }
}