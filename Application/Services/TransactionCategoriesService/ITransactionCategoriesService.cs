namespace Application.Services.TransactionCategoriesService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.DTOs.TransactionDTOs;

    public interface ITransactionCategoriesService
    {
        Task<List<TransactionCategoryDTO>> GetTransactionCategories(int userId);

        Task<List<TransactionCategoryDTO>> CreateCategory(int userId, string newCategory);

        Task<List<TransactionCategoryDTO>> RemoveCategory(int userId, int categoryId);
    }
}