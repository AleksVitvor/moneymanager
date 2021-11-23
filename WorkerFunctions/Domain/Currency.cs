namespace Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    [Table("Currencies")]
    public class Currency
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int CurrencyId { get; set; }

        [Column("Code")]
        [Required]
        public string CurrencyCode { get; set; }

        public virtual List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public virtual List<ExchangeRates> ExchangeRates { get; set; } = new List<ExchangeRates>();
    }
}