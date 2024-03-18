using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<GetAccountDTO>> GetAllAccountsAsync();

        Task<GetAccountDTO?> GetAccountByIdAsync(int accountId);
        Task<GetAccountDTO?> GetAccountByEmailAsync(string email);
        Task<GetAccountDTO?> GetAccountByEmailAndPasswordAsync(string email, string password);
        Task<GetAccountDTO?> GetAccountByUsernameAsync(string username);

        Task<bool> CreateAccountAsync(CreateAccountDTO account);

        Task<bool> UpdateAccountAsync(int id, UpdateAccountDTO getAccountDTO);

        Task<bool> DeleteAccountAsync(int accountId);

        Task<bool> UpdateAccountStatuslAsync(int accountId);
    }
}