namespace CustomersManagementSystem.ViewModels.Invoices;

public class InvoiceViewModel
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public Guid CustomerId { get; set; }

    [Display(Name = "Customer Name")]
    public string CustomerName { get; set; }
    public decimal Discount { get; set; }
    [Display(Name = "Total")]
    public decimal Total { get; set; }

    [Display(Name = "Already Paid")]
    public bool IsPaid { get; set; }

    [Display(Name = "Invoice Date")]
    public DateTime InvoiceDate { get; set; }
}
