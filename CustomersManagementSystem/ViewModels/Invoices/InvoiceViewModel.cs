namespace CustomersManagementSystem.ViewModels.Invoices
{
    public class InvoiceViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Guid CustomerId { get; set; }
        public float Discount { get; set; }
        public float Total { get; set; }
    }
}
