using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync();

        Task<Account?> GetAccountByIdAsync(int accountId);
        Task<Account?> GetAccountByEmailAsync(string email);
        Task<Account?> GetAccountByEmailAndPasswordAsync(string email, string password);

        Task<bool> CreateAccountAsync(Account account);

        Task<bool> UpdateAccountAsync(Account account);

        Task<bool> DeleteAccountAsync(int accountId);
    }
}