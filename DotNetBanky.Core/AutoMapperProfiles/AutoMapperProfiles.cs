using AutoMapper;
using DotNetBanky.Core.Constants;
using DotNetBanky.Core.DTOModels.Account;
using DotNetBanky.Core.DTOModels.Customer;
using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.DTOModels.Transaction;
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
                CreateMap<Customer, CustomerEditModel>().ReverseMap();
                CreateMap<Customer, CustomerListDTOModel>().ForMember(dto => dto.FullName, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname));
                CreateMap<Customer, CustomerDetailsDTOModel>().ForMember(dto => dto.Gender, opt => opt.MapFrom(src => GenderConstants.GenderList.First(v => v.Value == src.Gender).Key)).ReverseMap();
                CreateMap<CustomerDetailsDTOModel, CustomerEditModel>().ReverseMap();
                CreateMap<PagedResult<Customer>, PagedResult<CustomerListDTOModel>>().ReverseMap();

                // Account Mappings
                CreateMap<Account, AccountSummeryDTO>().ForMember(
                    dto => dto.AccountType,
                    opt => opt.MapFrom(src => src.Dispositions.First().Type)).ReverseMap();
                CreateMap<Account, AccountDetailsDTO>().ReverseMap();

                // Transaction
                CreateMap<Transaction, TransactionDTO>().ReverseMap();
                CreateMap<PagedResult<Transaction>, PagedResult<TransactionDTO>>().ReverseMap();
            }
        }
    }
}
