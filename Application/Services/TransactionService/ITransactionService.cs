using Application.DTOs.TransactionDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.TransactionService
{
    public interface ITransactionService
    {
        Task<List<TransactionDTO>> GetTransactions(int userId);

        Task<List<TransactionDTO>> CreateTransaction(IncommingTransactionDTO transactionDTO);

        Task<List<TransactionDTO>> UpdateTransaction(IncommingTransactionDTO transactionDTO);

        Task<List<TransactionDTO>> DeleteTransaction(int id, int userId);
    }
}
