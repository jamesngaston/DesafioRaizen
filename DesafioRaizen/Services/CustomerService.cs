using DesafioRaizen.Data;
using DesafioRaizen.Filters;
using DesafioRaizen.Models;
using DesafioRaizen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MR.AspNetCore.Pagination;
using MR.EntityFrameworkCore.KeysetPagination;

namespace DesafioRaizen.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DataContext _context;

        public CustomerService(DataContext context)
        {
            _context = context;
        }

        public async Task<Customer> Add(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var customer = await Get(id);
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<Customer> Get(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) throw new NullReferenceException("Cliente não encontrado.");
            else return customer;
        }

        public async Task<List<Customer>> GetPaged(CustomerFilter filter, Customer? customer)
        {
            KeysetPaginationContext<Customer> keysetContext;

            Action<KeysetPaginationBuilder<Customer>> action = filter.OrderBy switch
            {
                Filters.Enums.OrderBy.Id => filter.Ascending ? (b => b.Ascending(e => e.Id)) : (b => b.Descending(e => e.Id)),
                Filters.Enums.OrderBy.Name => filter.Ascending ? (b => b.Ascending(e => e.Name)) : (b => b.Descending(e => e.Name)),
                Filters.Enums.OrderBy.Email => filter.Ascending ? (b => b.Ascending(e => e.Email)) : (b => b.Descending(e => e.Email)),
                Filters.Enums.OrderBy.BirthDate => filter.Ascending ? (b => b.Ascending(e => e.BirthDate)) : (b => b.Descending(e => e.BirthDate)),
                Filters.Enums.OrderBy.CEP => filter.Ascending ? (b => b.Ascending(e => e.CEP)) : (b => b.Descending(e => e.CEP)),
                _ => filter.Ascending ? (b => b.Ascending(e => e.Id)) : (b => b.Descending(e => e.Id)),
            };

            keysetContext = _context.Customers.KeysetPaginate(action, reference: customer);
            var query = keysetContext.Query;

            if (filter.Name != null)
            {
                query = query.Where(x => x.Name.Contains(filter.Name));
            }
            if (filter.Email != null)
            {
                query = query.Where(x => x.Email.Contains(filter.Email));
            }
            if (filter.BirthDate != null)
            {
                query = query.Where(x => x.BirthDate == filter.BirthDate);
            }
            if (filter.CEP != null)
            {
                query = query.Where(x => x.CEP.Contains(filter.CEP));
            }
            var customers = await query.Take(20).ToListAsync();

            keysetContext.EnsureCorrectOrder(customers);

            var hasPrevious = await keysetContext.HasPreviousAsync(customers);

            var hasNext = await keysetContext.HasNextAsync(customers);

            return customers;
        }

        public async Task<Customer> Update(int id, Customer request)
        {
            request.Id = id;
            _context.Customers.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }
    }
}