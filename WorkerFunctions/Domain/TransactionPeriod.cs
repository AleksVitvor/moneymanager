namespace Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TransactionPeriods")]
    public class TransactionPeriod
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int TransactionPeriodId { get; set; }

        public string Description { get; set; }

        public virtual List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
