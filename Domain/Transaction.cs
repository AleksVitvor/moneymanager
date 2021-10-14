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
        [Column("TransactionDate")]
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

    }
}
