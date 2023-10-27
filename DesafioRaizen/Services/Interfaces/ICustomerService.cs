using DesafioRaizen.Filters;
using DesafioRaizen.Models;

namespace DesafioRaizen.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<List<Customer>> GetPaged(CustomerFilter filter, Customer? customer);

        public Task<Customer> Get(int id);

        public Task<Customer> Add(Customer customer);

        public Task<Customer> Update(int id, Customer customer);

        public Task<bool> Delete(int id);
    }
}