using AutoMapper;
using DotNetBanky.Core.DTOModels.Customer;
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
                CreateMap<UserCreateModel, User>().ForMember(usr => usr.UserName, opt => opt.MapFrom(dto => dto.DisplayName)).ReverseMap();
                CreateMap<UserDTOModel, User>().ForMember(usr => usr.UserName, opt => opt.MapFrom(dto => dto.DisplayName)).ReverseMap();

                CreateMap<Customer, CustomerCreateModel>().ReverseMap();
                CreateMap<Customer, CustomerListDTOModel>().ForMember(dto => dto.FullName, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname));
                CreateMap<Customer, CustomerDetailsDTOModel>().ReverseMap();
            }
        }
    }
}
