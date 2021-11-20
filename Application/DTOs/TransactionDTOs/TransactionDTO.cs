namespace Application.DTOs.TransactionDTOs
{
    using System;

    public class TransactionDTO
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public decimal StoredAmount { get; set; }

        public bool IsRepeatable { get; set; }

        public string TransactionType { get; set; }

        public string TransactionCategory { get; set; }

        public DateTime TransactionDate { get; set; }

        public int CurrencyId { get; set; }
    }
}
