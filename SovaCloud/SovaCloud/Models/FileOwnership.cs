namespace SovaCloud.Models
{
    public class FileOwnership
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int FileId { get; set; }
        public File File { get; set; } = null!;
    }
}
