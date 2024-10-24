using AutoMapper;
using PhoneAddressBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
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
}
