using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace artshare_server.Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await _dbContext.Accounts.Where(t => t.Email == email).FirstOrDefaultAsync();
        }
    }
}