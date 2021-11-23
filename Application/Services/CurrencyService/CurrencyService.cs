using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.TransactionDTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Services.CurrencyService
{
    public class CurrencyService : BaseService, ICurrencyService
    {
        public CurrencyService(MoneyManagerContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<CurrencyDTO>> GetCurrencyList()
        {
            try
            {
                return mapper.Map<List<CurrencyDTO>>(await context.Currencies.OrderBy(x => x.CurrencyId).ToListAsync());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}