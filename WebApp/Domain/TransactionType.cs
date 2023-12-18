namespace Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TransactionTypes")]
    public class TransactionType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int TransactionTypeId { get; set; }

        [Required]
        [Column("Description")]
        public string Description { get; set; }

        public virtual List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
