using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int TransactionId { get; set; }

        [Required]
        [Column("IsRepeatable")]
        public bool IsRepeatable { get; set; }

        [Required]
        [Column("Amount")]
        public float Amount { get; set; }

        [Required]
        [Column("TransactionTypeId")]
        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; }

        public virtual TransactionType TransactionType { get; set; }

        [Required]
        [Column("TransactionCategoryId")]
        [ForeignKey("TransactionCategory")]
        public int TransactionCategoryId { get; set; }

        public virtual TransactionCategory TransactionCategory { get; set; }

        [Required]
        [Column("TransactionDate", TypeName = "Date")]
        public DateTime TransactionDate { get; set; }

        [Column("ChildTransaction")]
        public int? ChildTransactionId { get; set; }

        public virtual Transaction ChildTransaction { get; set; }

        [Column("ParentTransaction")]
        public int? ParentTransactionId { get; set; }

        public virtual Transaction ParentTransaction { get; set; }

        [Required]
        [Column("UserId")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        [Column("CurrencyId")]
        [ForeignKey("Currency")]
        public int CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }

    }
}
