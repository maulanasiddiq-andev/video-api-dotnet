using AutoMapper;
using VideoApi.Dtos;
using VideoApi.Models;

namespace VideoApi.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterDto, UserModel>();
            CreateMap<UserModel, UserDto>();
        }
    }
}