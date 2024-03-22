﻿using artshare_server.ApiModels.DTOs;
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

        private readonly IAzureBlobStorageService _azureBlobStorageService;

        public AccountService(IUnitOfWork unitOfWork
                                , IAzureBlobStorageService azureBlobStorageService
                                , IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _azureBlobStorageService = azureBlobStorageService;
        }

        public async Task<IEnumerable<GetAccountDTO>> GetAllAccountsAsync()
        {
            var accountList = await _unitOfWork.AccountRepo.GetAccounts();
            return _mapper.Map<IEnumerable<GetAccountDTO>>(accountList);
        }

        public async Task<IEnumerable<UpdateAccountDTO>> SearchAccountsAsync(string usename)
        {
            try
            {
                var account = await _unitOfWork.AccountRepo.SearchAccountByUsername(usename);
                if (account != null)
                {
                    return _mapper.Map<IEnumerable<UpdateAccountDTO>>(account);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
        public async Task<GetAccountDTO?> GetAccountByIdAsync(int accountId)
        {
            if (accountId > 0)
            {
                var account = await _unitOfWork.AccountRepo.GetAccountById(accountId);
                return _mapper.Map<GetAccountDTO>(account);
            }
            return null;
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            var account = await _unitOfWork.AccountRepo.GetByEmailAsync(email);
            return account;
        }

        public async Task<GetAccountDTO?> GetAccountByEmailAndPasswordAsync(string email, string password)
        {
            var account = await _unitOfWork.AccountRepo.GetByEmailAsync(email);
            if(account != null)
            {
                if(BCrypt.Net.BCrypt.Verify(password, account.PasswordHash))
                {
                    return _mapper.Map<GetAccountDTO>(account);
                }
            }
            return null;
        }

        public async Task<bool> CreateAccountAsync(Account createAccountDTO)
        {
            try
            {
                await _unitOfWork.AccountRepo.AddAsync(_mapper.Map<Account>(createAccountDTO));
                var result = await _unitOfWork.SaveAsync() > 0;
                return result;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAccountAsync(UpdateAccountDTO updateAccountDTO)
        {
            try
            {
                var account = await _unitOfWork.AccountRepo.GetByIdAsync(updateAccountDTO.AccountId);
                if (account == null)
                {
                    return false;
                }
                account.Email = updateAccountDTO.Email;
                updateAccountDTO.Password = "123";
                account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateAccountDTO.Password);          
                account.UserName = updateAccountDTO.UserName;
                account.FullName = updateAccountDTO.FullName;
                account.PhoneNumber = updateAccountDTO.PhoneNumber;
                account.Status = (updateAccountDTO.Status.Equals("Active")) ? AccountStatus.Active : AccountStatus.Inactive;
                account.AvatarUrl = updateAccountDTO.AvatarUrl;
                _unitOfWork.AccountRepo.Update(account);
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

        public async Task<GetAccountDTO?> GetAccountByUsernameAsync(string username)
        {
            var account = await _unitOfWork.AccountRepo.GetByUsernameAsync(username);
            return _mapper.Map<GetAccountDTO>(account);
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