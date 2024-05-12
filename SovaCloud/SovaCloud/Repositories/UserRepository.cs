using Microsoft.EntityFrameworkCore;
using SovaCloud.Data;
using SovaCloud.Models;

namespace SovaCloud.Repositories
{
    public class UserRepository
    {
        private readonly SovaCloudDbContext _context;

        public UserRepository(SovaCloudDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAndPassword(string email, string password)
        {
            var query = _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EmailAddress == email && x.Password == password);

            if (query.Result != null)
            {
                return await query;
            }
            return null;
        }
    }
}
