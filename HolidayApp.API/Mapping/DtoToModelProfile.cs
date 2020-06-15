using AutoMapper;
using HolidayApp.API.Dtos;
using HolidayApp.API.Models;

namespace HolidayApp.API.Mapping
{
    public class DtoToModelProfile : Profile
    {
        public DtoToModelProfile()
        {
            CreateMap<UserForRegisterDto, User>();
        }
    }
}