using AutoMapper;
using DotNetBanky.Core.DTOModels.User;
using DotNetBanky.Core.Entities;

namespace DotNetBanky.Core.AutoMapperProfiles
{
    public static class AutoMapperProfiles
    {
        public class AutoMapperProfile : Profile
        {
            public AutoMapperProfile()
            {
                CreateMap<UserCreateModel, User>().ReverseMap();
            }
        }
    }
}
