namespace Application.Services.TransactionService
{
    using Application.DTOs.TransactionDTOs;
    using AutoMapper;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TransactionService : BaseService, ITransactionService
    {
        public TransactionService(MoneyManagerContext context, IMapper mapper)
            :base(context, mapper)
        {
        }

        public async Task<List<TransactionDTO>> GetTransactions(int userId)
        {
            try
            {
                var transactions = await context.Transactions
                    .Include(x => x.TransactionCategory)
                    .Include(x => x.TransactionType)
                    .Where(x => x.UserId == userId).ToListAsync();
                return mapper.Map<List<Transaction>, List<TransactionDTO>>(transactions);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TransactionDTO>> CreateTransaction(IncommingTransactionDTO transactionDTO)
        {
            try
            {
                await context.Transactions.AddAsync(mapper.Map<IncommingTransactionDTO, Transaction>(transactionDTO));
                await context.SaveChangesAsync();
                return await GetTransactions(transactionDTO.UserId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TransactionDTO>> UpdateTransaction(IncommingTransactionDTO transactionDTO)
        {
            try
            {
                context.Transactions.Update(mapper.Map<IncommingTransactionDTO, Transaction>(transactionDTO));
                await context.SaveChangesAsync();
                return await GetTransactions(transactionDTO.UserId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TransactionDTO>> DeleteTransaction(int id, int userId)
        {
            try
            {
                Transaction transaction = new() { TransactionId = id };
                var childTransaction = await context.Transactions.FirstOrDefaultAsync(x => x.ParentTransactionId == id);
                if (childTransaction != null)
                {
                    childTransaction.ParentTransactionId = null;
                    context.Transactions.Update(childTransaction);
                }

                var parentTransaction = await context.Transactions.FirstOrDefaultAsync(x => x.ChildTransactionId == id);
                if (parentTransaction != null)
                {
                    parentTransaction.ChildTransactionId = null;
                    context.Transactions.Update(parentTransaction);
                }

                context.Transactions.Attach(transaction);
                context.Transactions.Remove(transaction);
                await context.SaveChangesAsync();
                return await GetTransactions(userId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
