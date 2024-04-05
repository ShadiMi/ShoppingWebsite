using ShoppingWebsite.Models; // Adjust namespace to where your models are defined
using ShoppingWebsite.Repository; // Adjust namespace to where your repositories are defined
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingWebsite.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllCustomersAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(string customerId)
        {
            return await _customerRepository.GetCustomerByIdAsync(customerId);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            // Here you could add business logic before saving the customer.
            // For example, validating data, checking for duplicates, etc.
            await _customerRepository.AddCustomerAsync(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            // Business logic before updating the customer
            await _customerRepository.UpdateCustomerAsync(customer);
        }

        public async Task DeleteCustomerAsync(string customerId)
        {
            // Business logic before deleting the customer
            await _customerRepository.DeleteCustomerAsync(customerId);
        }
    }
}
