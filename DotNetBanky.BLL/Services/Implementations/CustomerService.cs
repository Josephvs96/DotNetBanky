using AutoMapper;
using DotNetBanky.Core.DTOModels.Customer;
using DotNetBanky.Core.Entities;
using DotNetBanky.Core.Exceptions;
using DotNetBanky.DAL.Repositories.IRepositories;

namespace DotNetBanky.BLL.Services.Implementations
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper) : base(customerRepository)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        public async Task CreateCustomerAsync(CustomerCreateModel model)
        {
            var customer = _mapper.Map<Customer>(model);
            await _customerRepository.AddOneAsync(customer);
        }

        public Task DeleteCustomerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditCustomerAsync(CustomerEditModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CustomerListDTOModel>> GetAllCustomerListAsync(int pageNumber, int pageSize)
        {
            var customersList = await _customerRepository.GetListAsync(page: pageNumber, pageSize: pageSize);
            return _mapper.Map<IEnumerable<CustomerListDTOModel>>(customersList);
        }

        public async Task<CustomerDetailsDTOModel> GetCustomerDetailsAsync(int id)
        {
            var customer = await _customerRepository.GetOneByIdAsync(id);
            if (customer == null) throw new NotFoundException("Customer could not be found");
            return _mapper.Map<CustomerDetailsDTOModel>(customer);
        }
    }
}
