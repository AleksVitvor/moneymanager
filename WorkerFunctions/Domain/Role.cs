namespace Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Roles")]
    public class Role
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int RoleId { get; set; }

        [Required]
        [Column("Description")]
        public string Description { get; set; }
    }
}
