namespace Application.Services.CurrencyService
{
    using DTOs.TransactionDTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICurrencyService
    {
        Task<List<CurrencyDTO>> GetCurrencyList();
    }
}