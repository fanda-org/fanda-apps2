using AutoMapper;
using Fanda.Accounting.Domain;
using Fanda.Accounting.Repository.Dto;
using System.Linq;

namespace Fanda.Accounting.Repository.AutoMapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<Location, LocationDto>()
            //    .ReverseMap();
            CreateMap<Contact, ContactDto>()
                .ForMember(vm => vm.IsDeleted, opt => opt.Ignore())
                .ForMember(vm => vm.Index, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Address, AddressDto>()
                .ReverseMap();
            CreateMap<PartyCategory, PartyCategoryDto>()
                .ReverseMap();
            CreateMap<ProductCategory, ProductCategoryDto>()
                .ReverseMap();
            CreateMap<Unit, UnitDto>()
                .ReverseMap();
            CreateMap<UnitConversion, UnitConversionDto>()
                .ReverseMap();
            CreateMap<ProductBrand, ProductBrandDto>()
                .ReverseMap();
            CreateMap<ProductSegment, ProductSegmentDto>()
                .ReverseMap();
            CreateMap<ProductVariety, ProductVarietyDto>()
                .ReverseMap();
            CreateMap<ProductIngredient, ProductIngredientDto>()
                .ReverseMap();
            CreateMap<ProductPricing, ProductPricingDto>()
                .ReverseMap();
            CreateMap<ProductPricingRange, ProductPricingRangeDto>()
                .ReverseMap();
            CreateMap<Product, ProductDto>()
                .ForMember(vm => vm.IsCompoundProduct, opt => opt.MapFrom(src => src.ParentIngredients.Any()))
                .ForMember(vm => vm.Ingredients, opt => opt.MapFrom(src => src.ParentIngredients))
                .ForMember(vm => vm.ProductPricings, opt => opt.MapFrom(src => src.ProductPricings))
                .ReverseMap();

            CreateMap<Bank, BankDto>()
                //.ForMember(dest => dest.Owner, opt => opt.Ignore())
                //.ForMember(dest => dest.OwnerId, opt => opt.Ignore())
                //.ForMember(vm => vm.IsDeleted, opt => opt.Ignore())
                //.ForMember(vm => vm.Index, opt => opt.Ignore())
                //.AfterMap((src, dest) =>
                //{
                //    //Guid partyId = Guid.Empty;
                //    if (src.PartyBanks != null && src.PartyBanks.Any())
                //    {
                //        var partyId = src.PartyBanks.FirstOrDefault().PartyId;
                //        if (partyId != null && partyId != Guid.Empty)
                //        {
                //            dest.OwnerId = partyId;
                //            dest.Owner = AccountOwner.Party;
                //        }
                //    }
                //    else if (src.OrgBanks != null && src.OrgBanks.Any())
                //    {
                //        var orgId = src.OrgBanks.FirstOrDefault().OrgId;
                //        if (orgId != null && orgId != Guid.Empty)
                //        {
                //            dest.OwnerId = orgId;
                //            dest.Owner = AccountOwner.Organization;
                //        }
                //    }
                //})
                .ReverseMap()
                .ForMember(x => x.AddressId, opt => opt.MapFrom(vm => vm.Address.Id))
                .ForMember(x => x.ContactId, opt => opt.MapFrom(vm => vm.Contact.Id));

            CreateMap<Organization, OrganizationDto>()
                //.ForPath(vm => vm.Users, opt => opt.MapFrom(src => src.OrgUsers.Select(ou => ou.User).ToList()))
                .ForPath(vm => vm.Contacts, opt => opt.MapFrom(src => src.OrgContacts.Select(c => c.Contact).ToList()))
                .ForPath(s => s.Addresses, opt => opt.MapFrom(src => src.OrgAddresses.Select(a => a.Address).ToList()))
                //.ForPath(vm => vm.Banks, opt => opt.MapFrom(src => src.Banks.Select(b => b.BankAccount).ToList()))
                .ReverseMap()
                .ForMember(x => x.OrgContacts,
                    src => src.MapFrom((orgVM, org, oc, context) =>
                      {
                          return orgVM.Contacts?.Select(c => new OrgContact
                          {
                              OrgId = orgVM.Id,
                              Organization = org,
                              ContactId = c.Id,
                              Contact = context.Mapper.Map<ContactDto, Contact>(c)
                          }).ToList();
                      }))
                .ForMember(x => x.OrgAddresses,
                    src => src.MapFrom((orgVM, org, oa, context) =>
                    {
                        return orgVM.Addresses?.Select(a => new OrgAddress
                        {
                            OrgId = orgVM.Id,
                            Organization = org,
                            AddressId = a.Id,
                            Address = context.Mapper.Map<AddressDto, Address>(a)
                        }).ToList();
                    }));
            //.ForMember(x => x.Banks,
            //    src => src.MapFrom((orgVM, org, i, context) =>
            //    {
            //        return orgVM.Banks?.Select(b => new OrgBank
            //        {
            //            OrgId = new Guid(orgVM.OrgId),
            //            Organization = org,
            //            BankAcctId = b.BankAcctId,
            //            BankAccount = context.Mapper.Map<BankDto, Bank>(b)
            //        }).ToList();
            //    }));

            CreateMap<Party, PartyDto>()
                .ForMember(vm => vm.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForPath(vm => vm.Contacts, m => m.MapFrom(s => s.PartyContacts.Select(pc => pc.Contact).ToList()))
                .ForPath(vm => vm.Addresses, m => m.MapFrom(s => s.PartyAddresses.Select(pa => pa.Address).ToList()))
                //.ForPath(vm => vm.Banks, m => m.MapFrom(s => s.Banks.Select(pb => pb.BankAccount).ToList()))
                .ReverseMap()
                .ForMember(x => x.Category, opt => opt.Ignore())
                .ForMember(x => x.PartyContacts,
                    src => src.MapFrom((partyVM, party, i, context) =>
                    {
                        return partyVM.Contacts?.Select(c => new PartyContact
                        {
                            PartyId = partyVM.LedgerId,
                            Party = party,
                            ContactId = c.Id,
                            Contact = context.Mapper.Map<ContactDto, Contact>(c)
                        }).ToList();
                    }))
                .ForMember(x => x.PartyAddresses,
                    src => src.MapFrom((partyVM, party, i, context) =>
                    {
                        return partyVM.Addresses?.Select(a => new PartyAddress
                        {
                            PartyId = partyVM.LedgerId,
                            Party = party,
                            AddressId = a.Id,
                            Address = context.Mapper.Map<AddressDto, Address>(a)
                        }).ToList();
                    }));
            //.ForMember(x => x.Banks,
            //    src => src.MapFrom((partyVM, party, i, context) =>
            //    {
            //        return partyVM.Banks?.Select(b => new PartyBank
            //        {
            //            PartyId = new Guid(partyVM.PartyId),
            //            Party = party,
            //            BankAcctId = b.BankAcctId,
            //            BankAccount = context.Mapper.Map<BankDto, Bank>(b)
            //        }).ToList();
            //    }));

            CreateMap<InvoiceCategory, InvoiceCategoryDto>()
                .ReverseMap();

            CreateMap<Stock, StockDto>()
                .ReverseMap();
            CreateMap<InvoiceItem, InvoiceItemDto>()
                .ReverseMap();
            CreateMap<Invoice, InvoiceDto>()
                .ReverseMap();

            CreateMap<AccountYear, AccountYearDto>()
                .ReverseMap();

            #region List mappings

            CreateMap<AccountYear, YearListDto>();
            CreateMap<Organization, OrgListDto>();
            CreateMap<Organization, OrgYearListDto>()
                .ForMember(vm => vm.SelectedYearId, opt => opt.Ignore())
                .ForMember(vm => vm.IsSelected, opt => opt.Ignore());
            CreateMap<PartyCategory, PartyCategoryListDto>();
            CreateMap<ProductCategory, ProductCategoryListDto>();
            CreateMap<Unit, UnitListDto>();
            CreateMap<ProductBrand, ProductBrandListDto>();

            #endregion List mappings
        }
    }

    //public class BankViewModelConverter : ITypeConverter<BankAccount, BankAccountViewModel>
    //{
    //    public BankAccountViewModel Convert(BankAccount source, BankAccountViewModel destination, ResolutionContext context)
    //    {
    //        return new BankAccountViewModel
    //        {
    //            AccountId = source.Id,
    //            Owner = source.PartyBanks.Any() ? Common.Enums.Owner.Party :
    //                (source.OrgBanks.Any() ? Common.Enums.Owner.Organization : Common.Enums.Owner.None),
    //            OwnerId = source.PartyBanks.Any() ? source.PartyBanks.FirstOrDefault()?.PartyId :
    //                (source.OrgBanks.Any() ? source.OrgBanks.FirstOrDefault()?.OrgId : Guid.Empty)
    //        };
    //    }
    //}
}
