using AutoMapper;
using EvaluationPartyProduct.DTO;
using EvaluationPartyProduct.Models;

namespace EvaluationPartyProduct.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<TblParty, PartyDTO>().ReverseMap();
            CreateMap<PartyCreationDTO, TblParty>();
            CreateMap<TblProduct, ProductDTO>().ReverseMap();
            CreateMap<ProductCreationDTO, TblProduct>();
            CreateMap<TblProductRate, ProductRateRelationDTO>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName)).ReverseMap();
            CreateMap<ProductRateCreationDTO, TblProductRate>();
            CreateMap<TblAssignParty, AssignPartyDTO>().ReverseMap();
            CreateMap<AssignPartyCreationDTO, TblAssignParty>();
            CreateMap<TblAssignParty, AssignPartyRelationDTO>().ForMember(dest => dest.PartyName, opt => opt.MapFrom(src => src.Party.PartyName)).ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName)).ReverseMap();
            CreateMap<TblInvoice, InvoiceDTO>().ForMember(dest=>dest.PartyName, opt => opt.MapFrom(src => src.Party.PartyName));
        }
    }
}
