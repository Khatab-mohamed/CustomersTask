using System.ComponentModel.DataAnnotations;

namespace CustomersManagementSystem.ViewModels;

public class CustomerViewModel
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }

    [Required]
    public string Address { get; set; }

    public string Phone { get; set; }
}