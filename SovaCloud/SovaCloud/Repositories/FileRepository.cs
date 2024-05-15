using Microsoft.EntityFrameworkCore;
using SovaCloud.Controllers;
using SovaCloud.Data;
using SovaCloud.Models;

namespace SovaCloud.Repositories
{
	public class FileRepository
	{
		private readonly SovaCloudDbContext _context;

        public FileRepository(SovaCloudDbContext context)
        {
            _context = context;
        }

        public async void AddNew(IFormFile file, string s3BucketWay)
        {
            var newFile = new Models.File
            {
                Name = file.FileName,
                Size = file.Length / 1024,
                Type = file.ContentType,
                DateTimeAdded = DateTime.Now,
                S3BucketWay = $"{s3BucketWay}"
            };

            _context.Files.Add(newFile);
            await _context.SaveChangesAsync();
        }

        public async Task<Models.File?> GetByName(string fileName)
        {
            var query = _context.Files
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == fileName);
            
			if (query != null)
			{
				return await query;
			}
			return null;
		}
    }
}
