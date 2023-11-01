using Azure;
using DesafioRaizen.Models;
using MR.AspNetCore.Pagination;

namespace DesafioRaizen.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly List<Customer> _customerList;
        private readonly Mock<DataContext> _context;
        private readonly Mock<DbSet<Customer>> _customers;
        private readonly ICustomerService _service;

        public CustomerServiceTests()
        {
            _customerList = new();
            for (int i = 0; i < 123; i++)
            {
                _customerList.Add(new Customer
                {
                    Id = i,
                    Name = $"Foo{i}",
                    Email = $"foo{i}@foo.com",
                    BirthDate = new DateTime(1900 + i, 1, 1),
                    CEP = $"{01001000 + i}"
                });
            };

            _customers = _customerList.AsQueryable().BuildMockDbSet();
            _context = new Mock<DataContext>();
            _context.Setup(x => x.Customers).Returns(_customers.Object);
            _service = new CustomerService(_context.Object);
        }

        [Fact]
        public async void GetThrowNull()
        {
            _customers.Setup(x => x.FindAsync(-1)).ReturnsAsync(_customerList.Find(x => x.Id == -1));
            await Assert.ThrowsAsync<NullReferenceException>(() => _service.Get(-1));
        }

        [Fact]
        public async void GetById()
        {
            int id = new Random().Next(0, 123);
            _customers.Setup(x => x.FindAsync(id)).ReturnsAsync(_customerList.Find(x => x.Id == id));
            Customer customer = await _service.Get(id);
            Assert.NotNull(customer);
            Assert.Equal(_customerList[id], customer);
        }

        [Fact]
        public async void GetPagedLastPage()
        {
            List<Customer> customers = await _service.GetPaged(new Filters.CustomerFilter(), null);
            Assert.NotNull(customers);
            Assert.Equal(20, customers.Count);
            Assert.Equal(_customerList.TakeLast(20).OrderByDescending(x => x.Id), customers);
        }

        [Fact]
        public async void GetPagedFirstPage()
        {
            List<Customer> customers = await _service.GetPaged(new Filters.CustomerFilter { Ascending = true }, null);
            Assert.NotNull(customers);
            Assert.Equal(20, customers.Count);
            Assert.Equal(_customerList.Take(20), customers);
        }

        [Fact]
        public async void GetPagedSecondPage()
        {
            List<Customer> customers = await _service.GetPaged(new Filters.CustomerFilter { Ascending = true }, _customerList[19]);
            Assert.NotNull(customers);
            Assert.Equal(20, customers.Count);
            Assert.Equal(_customerList.Skip(20).Take(20), customers);
        }

        [Fact]
        public async void GetPagedBeforeLastPage()
        {
            List<Customer> customers = await _service.GetPaged(new Filters.CustomerFilter(), _customerList[_customerList.Count - 20]);
            Assert.NotNull(customers);
            Assert.Equal(20, customers.Count);
            Assert.Equal(_customerList.OrderByDescending(x => x.Id).Skip(20).Take(20), customers);
        }

        [Fact]
        public async void GetPagedFilterSingle()
        {
            List<Customer> customers = await _service.GetPaged(new Filters.CustomerFilter { Ascending = true, Name = "Foo24" }, null);
            Assert.NotNull(customers);
            Assert.Single(customers);
            Assert.Contains(_customerList[24], customers);
        }

        [Fact]
        public async void GetPagedFilterAll()
        {
            List<Customer> customers = await _service.GetPaged(new Filters.CustomerFilter { Ascending = true, Name = "Foo" }, null);
            Assert.NotNull(customers);
            Assert.Equal(20, customers.Count);
            Assert.Equal(_customerList.Take(20), customers);
        }

        [Fact]
        public async void Add()
        {
            int i = 123;
            Customer customer = new Customer
            {
                Name = $"Foo{i}",
                Email = $"foo{i}@foo.com",
                BirthDate = new DateTime(1900 + i, 1, 1),
                CEP = $"{01001000 + i}"
            };

            _customers.Setup(x => x.Add(It.IsAny<Customer>()));
            Customer response = await _service.Add(customer);
            _customers.Verify(x => x.Add(customer));
            Assert.Equal(response, customer);
        }

        [Fact]
        public async void Update()
        {
            int i = 124;
            Customer request = new Customer
            {
                Name = $"Foo{i}",
                Email = $"foo{i}@foo.com",
                BirthDate = new DateTime(1900 + i, 1, 1),
                CEP = $"{01001000 + i}"
            };
            _customers.Setup(x => x.FindAsync(1)).ReturnsAsync(_customerList.Find(x => x.Id == 1));
            _customers.Setup(x => x.Update(It.IsAny<Customer>()));
            Customer updated = await _service.Update(1, request);

            _customers.Verify(x => x.Update(request));
        }

        [Fact]
        public async void Delete()
        {
            _customers.Setup(x => x.FindAsync(1)).ReturnsAsync(_customerList.Find(x => x.Id == 1));
            _customers.Setup(x => x.Remove(It.IsAny<Customer>()));
            bool deleted = await _service.Delete(1);

            _customers.Verify(x => x.Remove(_customerList[1]));
        }
    }
}