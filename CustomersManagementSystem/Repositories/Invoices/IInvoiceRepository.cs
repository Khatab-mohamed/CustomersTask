namespace CustomersManagementSystem.Repositories.Invoices;

public interface IInvoiceRepository
{
    Task<bool> IsExistAsync(Guid id);
    Task<Invoice?> GetAsync(Guid id);
    Task<IEnumerable<Invoice>> GetAsync();

    void Add(Invoice customer);
    void Update(Invoice customer);
    void Delete(Guid customerId);
    Task<bool> SaveAsync();
}
