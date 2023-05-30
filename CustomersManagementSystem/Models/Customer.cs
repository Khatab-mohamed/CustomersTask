using System.ComponentModel.DataAnnotations;

namespace CustomersManagementSystem.Models;

// Customer model
public class Customer
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    [Required]
    public string Address { get; set; }

    public string Phone { get; set; }

}
