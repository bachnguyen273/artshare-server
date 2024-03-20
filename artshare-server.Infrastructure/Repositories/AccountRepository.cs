using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections;

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

        public async Task<List<GetAccountDTO>> GetAccounts()
        {
            List<Account> account = await _dbContext.Accounts
                                       .Include(x => x.Orders)
                                            .ThenInclude(o => o.Artwork)
                                       .Include(x => x.Artworks)
                                       .Include(x => x.Comments)
                                       .Include(x => x.Likes)
                                       .ToListAsync();
            return _mapper.Map<List<GetAccountDTO>>(account);
        }
    }
}