using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;

namespace artshare_server.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            var accountList = await _unitOfWork.AccountRepo.GetAllAsync();
            return accountList;
        }

        public async Task<Account> GetAccountByIdAsync(int accountId)
        {
            if (accountId > 0)
            {
                var account = await _unitOfWork.AccountRepo.GetByIdAsync(accountId);
                if (account != null)
                {
                    return account;
                }
            }
            return null;
        }

        public async Task<bool> CreateAccountAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAccountAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}