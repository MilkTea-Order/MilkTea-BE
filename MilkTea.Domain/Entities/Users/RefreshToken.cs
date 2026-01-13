using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Domain.Entities.Users
{
    [Table("refreshtokens")]
    public class RefreshToken
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [Column("UserID")]
        public int UserId { get; set; }

        [Required]
        [Column("Token")]
        [MaxLength(500)]
        public string Token { get; set; } = default!;

        [Required]
        [Column("ExpiryDate")]
        public DateTime ExpiryDate { get; set; }

        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Column("LastUpdatedDate")]
        public DateTime? LastUpdatedDate { get; set; }

        [Required]
        [Column("IsRevoked")]
        public bool IsRevoked { get; set; } = false;

        // Navigation
        public User? User { get; set; }
    }
}
