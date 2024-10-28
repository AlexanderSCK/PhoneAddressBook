using AutoMapper;
using PhoneAddressBook.API.DTOs;
using PhoneAddressBook.Domain.Entities;

namespace PhoneAddressBook.Infrastructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Domain to DTO
        CreateMap<Person, PersonDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses));

        CreateMap<Address, AddressDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (int)src.Type))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressDetail))
            .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom(src => src.PhoneNumbers.Select(pn => pn.Number).ToList()));

        CreateMap<PhoneNumber, PhoneNumberDto>()
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number));

        // DTO to Domain
        CreateMap<CreatePersonDto, Person>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<CreateAddressDto, Address>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<CreatePhoneNumberDto, PhoneNumber>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdatePersonDto, Person>()
            .ForMember(dest => dest.Addresses, opt => opt.Ignore());

        CreateMap<UpdateAddressDto, Address>()
            .ForMember(dest => dest.PhoneNumbers, opt => opt.Ignore());

        CreateMap<UpdateAddressDto, Address>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.PersonId, opt => opt.Ignore()) 
            .ForMember(dest => dest.PhoneNumbers, opt => opt.Ignore());

        CreateMap<UpdatePhoneNumberDto, PhoneNumber>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.AddressId, opt => opt.Ignore()); 

        //Domain to Scaffolded models
        CreateMap<Person, Models.Person>()
            .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses));

        CreateMap<Address, Models.Address>()
            .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.AddressDetail))
            .ForMember(dest => dest.Phonenumbers, opt => opt.MapFrom(src => src.PhoneNumbers));

        CreateMap<PhoneNumber, Models.Phonenumber>()
            .ForMember(dest => dest.Phonenumber1, opt => opt.MapFrom(src => src.Number));

        CreateMap<Models.Person, Person>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Fullname))
            .ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.Addresses));

        CreateMap<Models.Address, Address>()
            .ForMember(dest => dest.AddressDetail, opt => opt.MapFrom(src => src.Address1))
            .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom(src => src.Phonenumbers));

        CreateMap<Models.Phonenumber, PhoneNumber>()
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Phonenumber1));
    }
}