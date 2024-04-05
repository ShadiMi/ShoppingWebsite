using ShoppingWebsite.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingWebsite.Repository
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(string customerId);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(string customerId);
    }
}
