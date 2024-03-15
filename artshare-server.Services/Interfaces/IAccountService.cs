using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<ProfileDTO>> GetAllAccountsAsync();

        Task<ProfileDTO?> GetAccountByIdAsync(int accountId);
        Task<Account?> GetAccountByEmailAsync(string email);
        Task<Account?> GetAccountByEmailAndPasswordAsync(string email, string password);
        Task<ProfileDTO?> GetAccountByUsernameAsync(string username);

        Task<bool> CreateAccountAsync(Account account);

        Task<bool> UpdateAccountAsync(int id, ProfileDTO profile);

        Task<bool> DeleteAccountAsync(int accountId);

        Task<bool> UpdateAccountStatuslAsync(int accountId);
    }
}