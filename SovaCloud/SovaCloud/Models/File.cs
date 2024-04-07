using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SovaCloud.Models
{
    public class File
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(6, 3)")]
        public decimal Size { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateTimeAdded { get; set; }

        [Required]
        public string S3BucketWay { get; set; } = null!;
    }
}
