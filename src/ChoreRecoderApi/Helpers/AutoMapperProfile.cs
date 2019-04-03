using AutoMapper;
using ChoreRecoderApi.Model;
using ChoreRecorderApi.Model;


namespace ChoreRecorderApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<RegisterUserDto,User>();
            CreateMap<User, RegisterUserDto >();
        }
    }
}