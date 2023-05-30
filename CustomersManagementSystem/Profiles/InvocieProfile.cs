using AutoMapper;

namespace CustomersManagementSystem.Profiles;

public class InvocieProfile : Profile
{
    public InvocieProfile()
    {
        CreateMap<InvoiceCreationViewModel, Invoice>()
            .ReverseMap();
        CreateMap<Invoice,InvoiceViewModel>()
            .ForMember(c => c.CustomerName, m => m.MapFrom(cvm => cvm.Customer.Name))
            .ReverseMap();
    }
}
