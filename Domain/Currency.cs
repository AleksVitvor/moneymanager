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

        [Column("CurrencySymbol")]
        [Required]
        public string CurrencySymbol { get; set; }

        public virtual List<Transaction> Transactions { get; set; } = new();

        public virtual List<ExchangeRates> ExchangeRates { get; set; } = new();
    }
}