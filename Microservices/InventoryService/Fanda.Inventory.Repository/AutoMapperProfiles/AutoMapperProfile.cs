using System.Linq;
using AutoMapper;
using Fanda.Inventory.Domain;
using Fanda.Inventory.Repository.Dto;

namespace Fanda.Inventory.Repository.AutoMapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
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

            CreateMap<InvoiceCategory, InvoiceCategoryDto>()
                .ReverseMap();

            CreateMap<Stock, StockDto>()
                .ReverseMap();
            CreateMap<InvoiceItem, InvoiceItemDto>()
                .ReverseMap();
            CreateMap<Invoice, InvoiceDto>()
                .ReverseMap();

            #region List mappings

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