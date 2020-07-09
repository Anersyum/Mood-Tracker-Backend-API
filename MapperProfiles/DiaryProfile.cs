using AutoMapper;
using SocialSite.API.Dto;
using SocialSite.API.Models;

namespace SocialSite.API.MapperProfiles
{
    public class DiaryProfile : Profile
    {
        public DiaryProfile()
        {
            CreateMap<Diary, DiaryToReturnDto>();
        }
    }
}