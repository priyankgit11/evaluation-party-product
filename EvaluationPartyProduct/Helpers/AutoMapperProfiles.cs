using AutoMapper;
using EvaluationPartyProduct.DTO;
using EvaluationPartyProduct.Models;

namespace EvaluationPartyProduct.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //party
            CreateMap<TblParty, PartyDTO>().ReverseMap();
            CreateMap<PartyCreationDTO, TblParty>();
            //product
            CreateMap<TblProduct, ProductDTO>().ReverseMap();
            CreateMap<ProductCreationDTO, TblProduct>();
            //product rate
            CreateMap<TblProductRate, ProductRateRelationDTO>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName)).ReverseMap();
            CreateMap<ProductRateCreationDTO, TblProductRate>();
            //assign party
            CreateMap<TblAssignParty, AssignPartyDTO>().ReverseMap();
            CreateMap<AssignPartyCreationDTO, TblAssignParty>();
            CreateMap<TblAssignParty, AssignPartyRelationDTO>().ForMember(dest => dest.PartyName, opt => opt.MapFrom(src => src.Party.PartyName)).ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName)).ReverseMap();
            //invoice
            CreateMap<TblInvoice, InvoiceDTO>().ForMember(dest=>dest.PartyName, opt => opt.MapFrom(src => src.Party.PartyName));
            CreateMap<TblInvoiceDetail, InvoiceDetailDTO>().ForMember(dest => dest.PartyName, opt => opt.MapFrom(src => src.Invoice.Party.PartyName)).ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName)).ReverseMap();
            CreateMap<TblInvoice, InvoiceCreationDTO>().ReverseMap();
            CreateMap<TblInvoiceDetail, InvoiceDetailCreationDTO>().ReverseMap();
        }
    }
}
