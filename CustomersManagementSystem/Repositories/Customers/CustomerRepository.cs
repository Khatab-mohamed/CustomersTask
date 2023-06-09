using CustomersManagementSystem.ViewModels;

namespace CustomersManagementSystem.Repositories.Customers;

public class CustomerRepository : ICustomerRepository
{
    #region Constructor
    private readonly ApplicationDbContext _context;
    public CustomerRepository(ApplicationDbContext context) => _context = context;

    #endregion

    public async Task<bool> IsExistAsync(Guid? customerId)
    {
        return await _context.Customers.AnyAsync(c => c.Id == customerId);
    }

    public void Add(Customer customer)
    {
        _context.Customers.Add(customer);
    }

    public void Delete(Guid customerId)
    {
        var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId);

        if (customer != null) _context.Customers.Remove(customer);
    }

    public async Task<Customer?> GetAsync(Guid? customerId)
    {
        var entity = await _context.Customers.FindAsync(customerId);
        return entity;
    }

    public IQueryable<Customer> GetAsync(CustomerParameters customersViewModel)
    {
        var customers =  _context.Customers
             as IQueryable<Customer>;

        //  Searching
        
        if (!string.IsNullOrEmpty(customersViewModel.SearchQuery))
        {
            var queryForWhereClause = customersViewModel.SearchQuery
                .Trim()
                .ToLowerInvariant();

            customers = customers.Where(c => c.Name.Contains(queryForWhereClause)
            ||  c.Address.Contains(queryForWhereClause));
        }

        // Sorting

        switch (customersViewModel.OrderBy)
        {
            case "Name":
                customers = customers.OrderBy(c => c.Name);
                break;
            case "Address":
                customers = customers.OrderBy(c => c.Address);
                break;
        }
       
        return customers;
    }

    public void Update(Customer customer)
    {
        _context.Customers.Update(customer);
    }

    public async Task<bool> SaveAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}