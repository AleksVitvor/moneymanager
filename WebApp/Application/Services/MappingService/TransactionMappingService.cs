using Application.DTOs.TransactionDTOs;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading.Tasks;

namespace Application.Services.MappingService
{
    public class TransactionMappingService : BaseService
    {
        public TransactionMappingService(MoneyManagerContext context, IMapper mapper) : base(context, mapper)
        { }

        public static decimal MapTransactionAmount(Transaction transaction)
        {
            return decimal.Round(transaction.TransactionTypeId == 2 ? (decimal)transaction.Amount : (decimal)(transaction.Amount * -1), 2);
        }
    }
}
