namespace CustomersManagementSystem.ViewModels;

public class CustomerParameters
{
    public string? SearchQuery { get; set; }
    const int maxPageSize = 20;
    public int PageNumber { get; set; } = 1;
    public int _pageSize = 5;
    public string OrderBy { get; set; } = "Name";
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > maxPageSize ? maxPageSize : value;
    }

}
