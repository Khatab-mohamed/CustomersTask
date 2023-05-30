namespace CustomersManagementSystem.Controllers;

public class CustomerController : Controller
{
    #region Constructor
    
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerController(
        ICustomerRepository customerRepository,
        IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }
    #endregion

    // GET: Customer
    public async Task<IActionResult> Index(
        string sortOrder,
        string searchQuery,
        int pageNumber,
        int pageSize)
    {
        #region Searching

        
        ViewData["CurrentFilter"] = searchQuery;

        var customersViewModel = new CustomerParameters();

        if (!string.IsNullOrEmpty(searchQuery))
        {
            customersViewModel.SearchQuery = searchQuery;
        
        }
        #endregion

        #region Sorting
        ViewData["NameSortParm"] = sortOrder;
        
        if (!string.IsNullOrEmpty(sortOrder))
        {
            customersViewModel.OrderBy = sortOrder;

        }

        #endregion


        var customersFromDb =  _customerRepository.GetAsync(customersViewModel);
 
        #region Paging


        var pagedList = await PaginatedList<Customer>
            .CreateAsync(customersFromDb.AsNoTracking(),
        pageNumber <= 0 ? 1:pageNumber,
        pageSize <=0 ? 5:pageSize);

        #endregion

        var customersToReturn = _mapper.Map<PaginatedList<CustomerViewModel>>(pagedList);
        return pagedList != null ?
                      View(customersToReturn) :
                      Problem("No Customers Yet");
    }

    public async Task<IActionResult> ExportExcel(PaginatedList<CustomerViewModel> customers)
    {
        if (customers.Count() > 0)
        {
            // Create the Excel package
            using (var package = new ExcelPackage())
            {
                // Add a new worksheet
                var worksheet = package.Workbook.Worksheets.Add("Customers");

                // Set column headers
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Address";
                worksheet.Cells[1, 3].Value = "Phone";

                // Fill data rows
                var row = 2;
                foreach (var customer in customers)
                {
                    worksheet.Cells[row, 1].Value = customer.Name;
                    worksheet.Cells[row, 2].Value = customer.Address;
                    worksheet.Cells[row, 3].Value = customer.Phone;
                    row++;
                }

                // Auto fit columns
                worksheet.Cells.AutoFitColumns();

                // Convert the package to a byte array
                var fileBytes = package.GetAsByteArray();

                // Set the content type and file name for the response
                var fileName = "Customers.xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                // Return the file as a downloadable attachment
                return File(fileBytes, contentType, fileName);
            }
        }
        else
        {
            TempData["Message"] = "No Data to Export";
            return View();
        }
    }

    // GET: Customer/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        if ( ! await _customerRepository.IsExistAsync(id))
        {
            return NotFound();
        }

        var customer = await _customerRepository.GetAsync(id);
        if (customer == null)
        {
            return NotFound();
        }
        var customerToReturn = _mapper.Map<CustomerViewModel>(customer);
        return View(customerToReturn);
    }

    // GET: Customer/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Customer/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Address,Phone")] CustomerCreationViewModel customer)
    {
        if (ModelState.IsValid)
        {
            var customerToAdd = _mapper.Map<Customer>(customer);
            _customerRepository.Add(customerToAdd);
            await _customerRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    // GET: Customer/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        if (!await _customerRepository.IsExistAsync(id))
        {
            return NotFound();
        }

        var customer = await _customerRepository.GetAsync(id);
        if (customer == null)
        {
            return NotFound();
        }
        var customerToReturn = _mapper.Map<CustomerViewModel>(customer);
        return View(customerToReturn);
    }

    // POST: Customer/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Address,phone")] CustomerViewModel customer)
    {
        if (id != customer.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var updatedCustomer = _mapper.Map<Customer>(customer);
            try
            {
                _customerRepository.Update(updatedCustomer);
                await _customerRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await _customerRepository.IsExistAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    // GET: Customer/Delete/5
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!await _customerRepository.IsExistAsync(id))
        {
             return NotFound();
        }

        var customer = await _customerRepository.GetAsync(id);
        if (customer == null)
        {
            return NotFound();
        }
        var customersToReturn = _mapper.Map<CustomerViewModel>(customer);

        return View(customersToReturn);
    }

    // POST: Customer/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (! await _customerRepository.IsExistAsync(id))
        {
            return Problem("There is no Customer");
        }
         _customerRepository.Delete(id);
        
        await _customerRepository.SaveAsync();
        return RedirectToAction(nameof(Index));
    }
}
