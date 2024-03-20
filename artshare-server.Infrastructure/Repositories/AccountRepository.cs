using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace artshare_server.Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly IMapper _mapper;
        public AccountRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public AccountRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<Account>> SearchAccountByUsername(string username)
        {
            return await _dbContext.Accounts.Where(x => x.UserName.Contains(username)).ToListAsync();
        }


        public async Task<GetAccountDTO> GetAccountById(int id)
        {
            Account account = _dbContext.Accounts
                                        .Include(x => x.Orders)
                                        .Include(x => x.Artworks)
                                        .Include(x => x.Watermarks)
                                        .Include(x => x.Comments)
                                        .Include(x => x.Likes)
                                        .FirstOrDefault(x => x.AccountId == id);
            return _mapper.Map<GetAccountDTO>(account);

        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await _dbContext.Accounts.Where(t => t.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Account?> GetByUsernameAsync(string username)
        {
            return await _dbContext.Accounts.Where(x => x.UserName == username).FirstOrDefaultAsync();
        }

    }
}