using SovaCloud.Data;
using SovaCloud.Models;

namespace SovaCloud.Repositories
{
	public class FileOwnershipRepository
	{
		private readonly SovaCloudDbContext _context;

		public FileOwnershipRepository(SovaCloudDbContext context)
		{
			_context = context;
		}

		public void AddNew(User user, Models.File file)
		{
			var newFileOwnership = new Models.FileOwnership
			{
				File = file,
				FileId = file.Id,
				User = user,
				UserId = user.Id
			};

			_context.FileOwnerships.Add(newFileOwnership);
			_context.SaveChanges();
		}
	}
}
