using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Enums;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;

namespace artshare_server.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            var accountList = await _unitOfWork.AccountRepo.GetAllAsync();
            return accountList;
        }

        public async Task<Account?> GetAccountByIdAsync(int accountId)
        {
            if (accountId > 0)
            {
                var account = await _unitOfWork.AccountRepo.GetByIdAsync(accountId);
                return account;
            }
            return null;
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            var account = await _unitOfWork.AccountRepo.GetByEmailAsync(email);
            return account;
        }

        public async Task<Account?> GetAccountByEmailAndPasswordAsync(string email, string password)
        {
            var account = await _unitOfWork.AccountRepo.GetByEmailAsync(email);
            if(account != null)
            {
                if(BCrypt.Net.BCrypt.Verify(password, account.PasswordHash))
                {
                    return account;
                }
            }
            return null;
        }

        public async Task<bool> CreateAccountAsync(Account account)
        {
            try
            {
                await _unitOfWork.AccountRepo.AddAsync(account);
                var result = await _unitOfWork.SaveAsync() > 0;
                return result;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAccountAsync(int id, ProfileDTO account)
        {
            try
            {
                var profile = await _unitOfWork.AccountRepo.GetByIdAsync(id);
                if (profile == null)
                {
                    return false;
                }
                profile.Email = account.Email;
                profile.PasswordHash = BCrypt.Net.BCrypt.HashPassword(account.PasswordHash);
                profile.AvatarUrl = account.AvatarUrl;
                profile.UserName = account.UserName;
                profile.FullName = account.FullName;
                profile.PhoneNumber = account.PhoneNumber;
                profile.Status = (account.Status.Equals("Active")) ? AccountStatus.Active : AccountStatus.Inactive;  
                _unitOfWork.AccountRepo.Update(profile);
                await _unitOfWork.SaveAsync();
                return true;
            }catch(DbUpdateException)
            {
                throw;
            }           
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<Account?> GetAccountByUsernameAsync(string username)
        {
            var account = await _unitOfWork.AccountRepo.GetByUsernameAsync(username);
            return account;
        }

        public async Task<bool> UpdateAccountStatuslAsync(int accountId)
        {
            try
            {
                var account = await _unitOfWork.AccountRepo.GetByIdAsync(accountId);
                if (account == null)
                {
                    return false;
                }
                account.Status = Core.Enums.AccountStatus.Inactive;
                _unitOfWork.AccountRepo.Update(account);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }            
        }
    }
}