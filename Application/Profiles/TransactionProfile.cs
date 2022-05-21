namespace Application.Profiles
{
    using AutoMapper;
    using Domain;
    using DTOs.TransactionDTOs;
    using Services.MappingService;

    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDTO>()
                .ForMember(res => res.Amount,
                    src => src.MapFrom(x => TransactionMappingService.MapTransactionAmount(x)))
                .ForMember(res => res.StoredAmount, src => src.MapFrom(x => x.Amount))
                .ForMember(res => res.TransactionDate, src => src.MapFrom(x => x.TransactionDate))
                .ForMember(res => res.TransactionCategory, src => src.MapFrom(x => x.TransactionCategory.Description))
                .ForMember(res => res.TransactionType, src => src.MapFrom(x => x.TransactionType.Description))
                .ForMember(res => res.IsRepeatable, src => src.MapFrom(x => x.IsRepeatable))
                .ForMember(res => res.Id, src => src.MapFrom(x => x.TransactionId))
                .ForMember(res => res.CurrencyId, src => src.MapFrom(x => x.CurrencyId));

            CreateMap<IncommingTransactionDTO, Transaction>()
                .ForMember(res => res.UserId, src => src.MapFrom(x => x.UserId))
                .ForMember(res => res.TransactionTypeId, src => src.MapFrom(x => x.TransactionTypeId))
                .ForMember(res => res.TransactionCategoryId, src => src.MapFrom(x => x.TransactionCategoryId))
                .ForMember(res => res.TransactionDate, src => src.MapFrom(x => x.TransactionDate))
                .ForMember(res => res.Amount, src => src.MapFrom(x => x.StoredAmount))
                .ForMember(res => res.IsRepeatable, src => src.MapFrom(x => x.IsRepeatable))
                .ForMember(res => res.TransactionId, src => src.MapFrom(x => x.Id))
                .ForMember(res => res.CurrencyId, src => src.MapFrom(x => x.CurrencyId))
                .ForMember(res => res.TransactionPeriodId, src => src.MapFrom(x => x.IsRepeatable ? x.TransactionPeriodId : null));

            CreateMap<TransactionCategory, TransactionCategoryDTO>()
                .ForMember(res => res.Value, src => src.MapFrom(x => x.TransactionCategoryId))
                .ForMember(res => res.ViewValue, src => src.MapFrom(x => x.Description));

            CreateMap<Currency, CurrencyDTO>()
                .ForMember(res => res.Value, src => src.MapFrom(x => x.CurrencyId))
                .ForMember(res => res.ViewValue, res => res.MapFrom(x => x.CurrencyCode));
        }
    }
}
