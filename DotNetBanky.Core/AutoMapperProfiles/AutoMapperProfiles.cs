using AutoMapper;
using DotNetBanky.Core.Constants;
using DotNetBanky.Core.DTOModels.Account;
using DotNetBanky.Core.DTOModels.Customer;
using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.DTOModels.Search;
using DotNetBanky.Core.DTOModels.Transaction;
using DotNetBanky.Core.DTOModels.User;
using DotNetBanky.Core.Entities;
using DotNetBanky.Core.SearchEntities;

namespace DotNetBanky.Core.AutoMapperProfiles
{
    public static class AutoMapperProfiles
    {
        public class UserProfile : Profile
        {
            public UserProfile()
            {
                // User Mappings
                CreateMap<UserCreateModel, User>().ForMember(usr => usr.UserName, opt => opt.MapFrom(dto => dto.Email)).ReverseMap();
                CreateMap<UserDTOModel, User>().ForMember(usr => usr.UserName, opt => opt.MapFrom(dto => dto.Email)).ReverseMap();
            }
        }

        public class CustomerProfile : Profile
        {
            public CustomerProfile()
            {
                // Customer Mappings
                CreateMap<Customer, CustomerCreateModel>().ReverseMap();
                CreateMap<Customer, CustomerEditModel>().ReverseMap();
                CreateMap<Customer, CustomerListDTOModel>().ForMember(dto => dto.FullName, opt => opt.MapFrom(src => src.Givenname + " " + src.Surname));
                CreateMap<Customer, CustomerDetailsDTOModel>().ForMember(dto => dto.Gender, opt => opt.MapFrom(src => GenderConstants.GenderList.First(v => v.Value == src.Gender).Key)).ReverseMap();
                CreateMap<CustomerDetailsDTOModel, CustomerEditModel>().ReverseMap();
                CreateMap<PagedResult<Customer>, PagedResult<CustomerListDTOModel>>().ReverseMap();
                CreateMap<CustomerCreateModel, UserCreateModel>()
                    .ForMember(u => u.FullName, opt => opt.MapFrom(c => $"{c.Givenname} {c.Surname}"))
                    .ForMember(u => u.DisplayName, opt => opt.MapFrom(c => $"{c.Givenname}.{c.Surname[0].ToString().ToUpper()}"))
                    .ForMember(u => u.Email, opt => opt.MapFrom(c => c.Emailaddress))
                    .ForMember(u => u.Role, opt => opt.MapFrom(src => RoleConstants.Customer));

                CreateMap<Customer, CustomerSearch>().ReverseMap();
                CreateMap<Customer, CustomerSearchDTO>().ReverseMap();
                CreateMap<CustomerSearch, CustomerSearchDTO>().ReverseMap();
                CreateMap<PagedResult<Customer>, PagedResult<CustomerSearch>>().ReverseMap();
                CreateMap<PagedResult<Customer>, PagedResult<CustomerSearchDTO>>().ReverseMap();
                CreateMap<PagedResult<CustomerSearch>, PagedResult<CustomerSearchDTO>>().ReverseMap();
            }
        }

        public class AccountProfile : Profile
        {
            public AccountProfile()
            {
                // Account Mappings
                CreateMap<Account, AccountSummeryDTO>().ForMember(
                    dto => dto.AccountType,
                    opt => opt.MapFrom(src => src.Dispositions.First().Type)).ReverseMap();
                CreateMap<Account, AccountDetailsDTO>().ReverseMap();
                CreateMap<Account, AccountCreateModel>().ReverseMap();
            }
        }

        public class TransactionProfile : Profile
        {
            public TransactionProfile()
            {
                // Transaction
                CreateMap<Transaction, TransactionDTO>().ReverseMap();
                CreateMap<PagedResult<Transaction>, PagedResult<TransactionDTO>>().ReverseMap();
                CreateMap<TransactionCreateDTO, Transaction>().ReverseMap();
            }
        }
    }
}
