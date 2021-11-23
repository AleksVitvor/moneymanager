namespace Application.DTOs.TransactionDTOs
{
    using System;

    public class IncommingTransactionDTO
    {
        public int Id { get; set; }

        public float StoredAmount { get; set; }

        public bool IsRepeatable { get; set; }

        public int TransactionTypeId { get; set; }

        public int TransactionCategoryId { get; set; }

        public DateTime TransactionDate { get; set; }

        public int UserId { get; set; }

        public int CurrencyId { get; set; }
    }
}
