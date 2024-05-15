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

        public async Task AddNew(IFormFile file, string s3BucketWay)
        {
            var newFile = new Models.File
            {
                Name = file.FileName,
                Size = file.Length / (decimal)1000000,
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
                .FirstOrDefaultAsync(x => x.Name == fileName);
            
			if (query != null)
			{
				return await query;
			}
			return null;
		}

        public async Task<List<Models.File>> GetListByEmail(string email)
        {
            var query = _context.Files
                .Where(x => x.S3BucketWay.Substring(0, email.Length) == email)
                .ToListAsync();

            if (query != null)
            {
                return await query;
            }
            return null;
        }

        public async Task DeleteOne(string fileName)
        {
            var query = _context.Files
                .FirstAsync(x => x.S3BucketWay == fileName);

            if (query != null) 
            {
                _context.Files.Remove(query.Result);
                await _context.SaveChangesAsync();
            }
        }
    }
}
