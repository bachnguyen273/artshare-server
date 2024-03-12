﻿using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Enums;
using artshare_server.Core.Interfaces;
using artshare_server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace artshare_server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccount()
        {
            try
            {
                var accList = await _accountService.GetAllAccountsAsync();
                if(accList == null)
                {
                    return BadRequest("List is null");
                }
                return Ok(accList);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetAccountByUsername(string username)
        {
            try
            {
                var account = await _accountService.GetAccountByUsernameAsync(username);
                if (account == null || account.Status.Equals(AccountStatus.Inactive))
                {
                    return NotFound();
                }
                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById (int id)
        {
            try
            {
                var account = await _accountService.GetAccountByIdAsync(id);
                if (account == null)
                {
                    return NotFound();
                }
                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile (int id, ProfileDTO profileDTO)
        {
            try
            {
                var check = await _accountService.GetAccountByIdAsync (id);
                if (check == null)
                {
                    return NotFound();
                }
                var up = await _accountService.UpdateAccountAsync(id,profileDTO);
                if (up)
                {
                    return Ok("Update SUCCESS!");
                }
                return BadRequest("Update FAIL");
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                var check = await _accountService.UpdateAccountStatuslAsync(id);
                if (!check)
                {
                    return BadRequest("Delete Fail");
                }
                return Ok("Delete Success");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
