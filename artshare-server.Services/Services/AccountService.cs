﻿using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Enums;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.FilterModels;
using artshare_server.Services.FilterModels.Helpers;
using artshare_server.Services.Interfaces;
using artshare_server.Services.Utils;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<PagedResult<GetAccountDTO>> GetAllAccountsAsync(AccountFilter filters)
        {
            // Apply filtering
            var items = await _unitOfWork.AccountRepo.GetAccounts();
            IQueryable<GetAccountDTO> filteredItemsQuery = items.AsQueryable();

            if (!string.IsNullOrEmpty(filters.FullName))
                filteredItemsQuery = filteredItemsQuery.Where(item => item.FullName.Contains(filters.FullName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(filters.UserName))
                filteredItemsQuery = filteredItemsQuery.Where(item => item.UserName.Contains(filters.UserName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(filters.Email))
                filteredItemsQuery = filteredItemsQuery.Where(item => item.Email.Contains(filters.Email, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(filters.PhoneNumber))
                filteredItemsQuery = filteredItemsQuery.Where(item => item.Email.Contains(filters.PhoneNumber, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(filters.Role.ToString()))
                filteredItemsQuery = filteredItemsQuery.Where(item => item.Role == filters.Role.ToString());
            // Apply sorting
            if (!string.IsNullOrEmpty(filters.SortBy))
            {
                switch (filters.SortBy)
                {
                    default:
                        // Handle other sorting filter using Utils.GetPropertyValue
                        filteredItemsQuery = filters.SortAscending ?
                            filteredItemsQuery.OrderBy(item => Helpers.GetPropertyValue(item, filters.SortBy)) :
                            filteredItemsQuery.OrderByDescending(item => Helpers.GetPropertyValue(item, filters.SortBy));
                        break;
                }
            }

            // Apply paging
            var pagedItems = filteredItemsQuery
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToList(); // Materialize the query

            return new PagedResult<GetAccountDTO>
            {
                Items = pagedItems,
                PageNumber = filters.PageNumber,
                PageSize = filters.PageSize,
                TotalItems = pagedItems.Count()
            };
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

        public async Task<bool> UpdateAccountAsync(int id, ProfileDTO updateAccountDTO)
        {
            try
            {
                var account = await _unitOfWork.AccountRepo.GetByIdAsync(updateAccountDTO.AccountId);
                if (account == null)
                {
                    return false;
                }
                account.Email = updateAccountDTO.Email;
                account.PasswordHash = (account.PasswordHash != null) ? BCrypt.Net.BCrypt.HashPassword(account.PasswordHash) : updateAccountDTO.PasswordHash;
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

		public async Task<string> CheckAccount(Account account)
		{
            try
            {
                var accounts = _unitOfWork.AccountRepo.GetAccounts().Result;
                if (accounts.FirstOrDefault(x => x.UserName == account.UserName) != null)
                    return "Username existed";
                if (accounts.FirstOrDefault(x => x.Email == account.Email) != null)
                    return "Email existed";
                if (accounts.FirstOrDefault(x => x.PhoneNumber == account.PhoneNumber) != null)
                    return "Phone existed";
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
		}
    }
}