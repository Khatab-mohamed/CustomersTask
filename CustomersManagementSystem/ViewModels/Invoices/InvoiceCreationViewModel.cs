using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CustomersManagementSystem.ViewModels.Invoices;

public class InvoiceCreationViewModel
{
    public string Description { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public float Discount { get; set; }
    [DisplayName("Customer Name")]
    public Guid CustomerId { get; set; }

    [Display(Name = "Already Paid")]
    public bool IsPaid { get; set; }
  
}
