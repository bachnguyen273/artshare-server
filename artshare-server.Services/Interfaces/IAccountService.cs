using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<GetAccountDTO>> GetAllAccountsAsync();

        Task<GetAccountDTO?> GetAccountByIdAsync(int accountId);
        Task<Account?> GetAccountByEmailAsync(string email);
        Task<GetAccountDTO?> GetAccountByEmailAndPasswordAsync(string email, string password);
        Task<GetAccountDTO?> GetAccountByUsernameAsync(string username);

        Task<bool> CreateAccountAsync(Account account);

        Task<bool> UpdateAccountAsync(UpdateAccountDTO getAccountDTO);

        Task<bool> DeleteAccountAsync(int accountId);

        Task<bool> UpdateAccountStatuslAsync(int accountId);
        Task<IEnumerable<UpdateAccountDTO>> SearchAccountsAsync(string username);
    }
}