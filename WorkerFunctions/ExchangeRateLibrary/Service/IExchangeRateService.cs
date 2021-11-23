using System;
using System.Threading.Tasks;
using ExchangeRateLibrary.Model;

namespace ExchangeRateLibrary.Service
{
    public interface IExchangeRateService
    {
        Task<ExchangeRateGeneralInfo> GetExchangeRate(DateTime date);
    }
}