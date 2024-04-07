using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SovaCloud.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string EmailAddress { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(6, 3)")]
        public decimal SpaceOccupied { get; set; }
    }
}
