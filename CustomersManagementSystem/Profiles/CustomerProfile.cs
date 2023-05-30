using AutoMapper;

namespace CustomersManagementSystem.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CustomerCreationViewModel, Customer>()
            .ForMember(c => c.Name, m => m.MapFrom(cvm => cvm.Name))
            .ForMember(d => d.Address, m => m.MapFrom(s => s.Address))
            .ForMember(d => d.Phone, m => m.MapFrom(s => s.Phone))
            .ReverseMap();

        CreateMap<CustomerViewModel, Customer>().ReverseMap();
    }
}
