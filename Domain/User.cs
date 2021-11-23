namespace Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Users")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int UserId { get; set; }

        [Required]
        [Column("Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Column("Password")]
        public string Password { get; set; }

        [Required]
        [Column("FullName")]
        public string FullName { get; set; }

        [Required]
        [Column("Username")]
        public string Username { get; set; }

        [Required]
        [Column("RoleId")]
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        public virtual List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public virtual List<TransactionCategory> TransactionCategories { get; set; } = new List<TransactionCategory>();
    }
}
