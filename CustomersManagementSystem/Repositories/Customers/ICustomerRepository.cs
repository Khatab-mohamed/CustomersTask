using CustomersManagementSystem.ViewModels;

namespace CustomersManagementSystem.Repositories.Customers;

public interface ICustomerRepository
{
    Task<bool> IsExistAsync(Guid? customerId);
    Task<Customer?> GetAsync(Guid? customerId);
    IQueryable<Customer> GetAsync(CustomerParameters customer);

    void Add(Customer customer);
    void Update(Customer customer);
    void Delete(Guid customerId);
    Task<bool> SaveAsync();
}
