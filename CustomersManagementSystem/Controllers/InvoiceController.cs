namespace CustomersManagementSystem.Controllers;

public class InvoiceController : Controller
{
    #region Constructor
    private readonly ApplicationDbContext _context;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;
    public InvoiceController(ApplicationDbContext context,
        IInvoiceRepository invoiceRepository,
        IMapper mapper)
    {
        _context = context;
        _invoiceRepository = invoiceRepository;
        _mapper = mapper;
    }
    #endregion

    // GET: Invoice
    public async Task<IActionResult> Index()
    {
        var invoices = await _invoiceRepository.GetAsync();
        var invoicesToReturn = _mapper.Map<IEnumerable<InvoiceViewModel>>(invoices);
        return View(invoicesToReturn);
    }

    // GET: Invoice/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        if (await _invoiceRepository.IsExistAsync(id) 
            || await _invoiceRepository.GetAsync() == null)
            return NotFound();
        
        var invoice = await _invoiceRepository.GetAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        return View(invoice);
        
    }

    // GET: Invoice/Create
    public IActionResult Create()
    {
        ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
        return View();
    }

    // POST: Invoice/Create

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Description,Quantity,Price,CustomerId,Discount,IsPaid")] InvoiceCreationViewModel invoice)
    {
        if (ModelState.IsValid)
        {
           var invoiceToAdd = _mapper.Map<Invoice>(invoice);
            invoiceToAdd.InvoiceDate = DateTime.UtcNow;
            invoiceToAdd.Total = invoiceToAdd.Quantity * invoiceToAdd.Price;
            invoiceToAdd.GrandTotal = invoiceToAdd.Total - ((invoiceToAdd.Total * invoiceToAdd.Discount) / 100);
            _invoiceRepository.Add(invoiceToAdd);
            
            await _invoiceRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", invoice.CustomerId);
        return View(invoice);
    }

    // GET: Invoice/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.Invoices == null)
        {
            return NotFound();
        }

        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }
        ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", invoice.CustomerId);
        return View(invoice);
    }
   
    // GET: Invoice/Delete/5

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.Invoices == null)
        {
            return NotFound();
        }

        var invoice = await _context.Invoices
            .Include(i => i.Customer)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (invoice == null)
        {
            return NotFound();
        }

        return View(invoice);
    }

    // POST: Invoice/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.Invoices == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Invoices'  is null.");
        }
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice != null)
        {
            _context.Invoices.Remove(invoice);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction("index");
    }
  
    public async Task<IActionResult> AddOrEdit(Guid? id)
    {
        if (id == null)
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            return View(new InvoiceViewModel());
        }
        else
        {
            //var invoiceFromDb = await _invoiceRepository.IsExistAsync(id);
            var invoiceFromDb = await _context.Invoices.FindAsync(id);
            
            if (invoiceFromDb == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            var invoiceViewModel = _mapper.Map<InvoiceViewModel>(invoiceFromDb);
            return View(invoiceViewModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddOrEdit(Guid id, [Bind("Description,Quantity,Price,CustomerId,Discount")]
    InvoiceCreationViewModel invoiceViewModel)
    {
        if (ModelState.IsValid)
        {
            //Insert
            if (id == Guid.Empty)
            {
                var invoiceToAdd = _mapper.Map<Invoice>(invoiceViewModel);
                invoiceToAdd.InvoiceDate = DateTime.UtcNow;
                invoiceToAdd.Total = invoiceToAdd.Quantity * invoiceToAdd.Price;
                invoiceToAdd.GrandTotal = invoiceToAdd.Total - ((invoiceToAdd.Total * invoiceToAdd.Discount) / 100);
                _invoiceRepository.Add(invoiceToAdd);

                await _invoiceRepository.SaveAsync();
            }
            //Update
            else
            {
                try
                {
                    var invoiceToEdit = _mapper.Map<Invoice>(invoiceViewModel);
                    invoiceToEdit.Id = id;
                    invoiceToEdit.Total = invoiceToEdit.Quantity * invoiceToEdit.Price;
                    invoiceToEdit.GrandTotal = invoiceToEdit.Total - ((invoiceToEdit.Total * invoiceToEdit.Discount) / 100);
                    _invoiceRepository.Update(invoiceToEdit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                   /* if (!TransactionModelExists(invoiceViewModel.TransactionId))
                    { return NotFound(); }
                    else
                    { throw; }*/ 
                }
            }
            //var invoices = await _invoiceRepository.GetAsync();

            //var invoicesToReturn = _mapper.Map<IEnumerable<InvoiceViewModel>>(invoices);

            return RedirectToAction("index");
        }
        return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", invoiceViewModel) });
    }
}
