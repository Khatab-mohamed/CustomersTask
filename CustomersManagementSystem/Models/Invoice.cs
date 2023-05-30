namespace CustomersManagementSystem.Models;

// Invoice model
public class Invoice
{
    public Guid Id { get; set; }

    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
   
    public decimal Discount { get; set; }
    public decimal Total { get; set; }

    [Display(Name = "Invoice Date")]
    [Required]
    public DateTime InvoiceDate { get; set; }

    [Display(Name = "Already Paid")]
    public bool IsPaid { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
}
