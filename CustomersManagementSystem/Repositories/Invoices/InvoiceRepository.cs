namespace CustomersManagementSystem.Repositories.Invoices;

public class InvoiceRepository : IInvoiceRepository
{
    #region Constructor
    private readonly ApplicationDbContext _context;
    public InvoiceRepository(ApplicationDbContext context) => _context = context;

    #endregion

    public async Task<bool> IsExistAsync(Guid invoiceId)
    {
        return await _context.Invoices
            .AnyAsync(c => c.Id == invoiceId);
    }

    public void Add(Invoice invoice)
    {
        _context.Invoices.Add(invoice);
    }

    public void Delete(Guid id)
    {
        var invoice = _context.Invoices.FirstOrDefault(c => c.Id == id);

        if (invoice != null) _context.Invoices.Remove(invoice);
    }

    public async Task<Invoice?> GetAsync(Guid invoiceId)
    {
        var entity = await _context.Invoices.FindAsync(invoiceId);
        return entity;
    }

    public async Task<IEnumerable<Invoice>> GetAsync()
    {
        var invoices = await _context.Invoices
             .Include(i => i.Customer)
             .ToListAsync();
        return invoices;
    }

    public void Update(Invoice invoice)
    {
        _context.Invoices.Update(invoice);
    }

    public async Task<bool> SaveAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}