using AutoMapper;
using SocialSite.API.Dto;
using SocialSite.API.Models;

namespace SocialSite.API.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserToListDto>();
            CreateMap<User, DetailedUserDto>();
        }
    }
}