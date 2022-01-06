using AutoMapper;
using DotNetBanky.Core.Constants;
using DotNetBanky.Core.DTOModels.Customer;
using DotNetBanky.Core.DTOModels.Paging;
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

                // User Mappings
                CreateMap<UserCreateModel, User>().ForMember(usr => usr.UserName, opt => opt.MapFrom(dto => dto.DisplayName)).ReverseMap();
                CreateMap<UserDTOModel, User>().ForMember(usr => usr.UserName, opt => opt.MapFrom(dto => dto.DisplayName)).ReverseMap();

                // Customer Mappings
                CreateMap<Customer, CustomerCreateModel>().ReverseMap();
                CreateMap<Customer, CustomerListDTOModel>().ForMember(dto => dto.FullName, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname));
                CreateMap<Customer, CustomerDetailsDTOModel>().ForMember(dto => dto.Gender, opt => opt.MapFrom(src => GenderConstants.GenderList.First(v => v.Value == src.Gender).Key)).ReverseMap();
                CreateMap<PagedResult<Customer>, PagedResult<CustomerListDTOModel>>().ReverseMap();
            }
        }
    }
}
