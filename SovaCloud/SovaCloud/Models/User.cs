using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SovaCloud.Models
{
    public class User
    {
        public int Id { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "E-mail address must be entered!")]
        [RegularExpression(@"^[\w-]{4,}(?:\.[\w-]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7}$", ErrorMessage = "Incorrect Email-address")]
        public string EmailAddress { get; set; } = null!;

        [Required(ErrorMessage = "Password must be entered!")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must consist of at least 8 characters and not exceed 20")]
        public string Password { get; set; } = null!;

        [NotMapped]
        public string PasswordConfirmation { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(6, 3)")]
        public decimal SpaceOccupied { get; set; }
    }
}
