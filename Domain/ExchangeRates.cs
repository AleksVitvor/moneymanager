namespace Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ExchangeRates")]
    public class ExchangeRates
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int ExchangeRateId { get; set; }

        [Column("ExchangeRate")]
        [Required]
        public double ExchangeRate { get; set; }

        [Required]
        [Column("Date", TypeName = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [Column("CurrencyId")]
        [ForeignKey("Currency")]
        public int CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }
    }
}