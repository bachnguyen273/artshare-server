
using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Models;

namespace artshare_server.Core.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account?> GetByEmailAsync(string email);
        Task<Account?> GetByUsernameAsync(string username);
        Task<GetAccountDTO> GetAccountById(int id);
    }
}