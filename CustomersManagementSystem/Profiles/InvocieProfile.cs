using AutoMapper;

namespace CustomersManagementSystem.Profiles;

public class InvocieProfile : Profile
{
    public InvocieProfile()
    {
        CreateMap<InvoiceCreationViewModel, Invoice>()
            .ReverseMap();
    }
}
