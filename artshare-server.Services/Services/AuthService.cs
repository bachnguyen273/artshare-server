using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Enums;
using artshare_server.Core.Models;
using artshare_server.Services.CustomExceptions;
using artshare_server.Services.FilterModels;
using artshare_server.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace artshare_server.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(IAccountService accountService, IMapper mapper, IConfiguration configuration)
        {
            _accountService = accountService;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(LoginDTO loginData)
        {
            var account = await _accountService.GetAccountByEmailAndPasswordAsync(loginData.Email, loginData.Password);
            if (account == null)
            {
                return null;
            }
            return CreateToken(account);
        }

        public async Task<bool> RegisterAsync(RegisterDTO registerData)
        {
            try
            {
                var check = await _accountService.CheckAccount(_mapper.Map<Account>(registerData));
                if (check != null)
                {
                    throw new RegistrationException(check);
                }
                var account = _mapper.Map<Account>(registerData);
                account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerData.Password);
                account.PaypalSercretKey = "ECO9h4desAjKAvg38eDoBjyunXniFe4uOT9hJFR7o0v4ezlqF0Gx2tqTD4WjHLOim2e9DPY2cMIYT52R";
                account.PaypalClientId = "AdF7Dojm2i_I9lvhhaCkegClqMVktUyxQoX7s1zYBj-2vxsoXngbYgqOHo6XkYxHsFVZYgYEkpo1f4pM";
                var result = await _accountService.CreateAccountAsync(account);
                return result;
            }
            catch (DbUpdateException ex)
            {
                throw new RegistrationException(ex.Message);
            }
        }

        private string CreateToken(GetAccountDTO account)
        {
            var nowUtc = DateTime.UtcNow;
            var expirationDuration = TimeSpan.FromMinutes(60);
            var expirationUtc = nowUtc.Add(expirationDuration);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,
                        _configuration.GetSection("JwtSecurityToken:Subject").Value),
                new Claim(JwtRegisteredClaimNames.Jti,
                                  Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                                  EpochTime.GetIntDate(nowUtc).ToString(),
                                  ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Exp,
                                  EpochTime.GetIntDate(expirationUtc).ToString(),
                                  ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Iss,
                                  _configuration.GetSection("JwtSecurityToken:Issuer").Value),
                new Claim(JwtRegisteredClaimNames.Aud,
                                  _configuration.GetSection("JwtSecurityToken:Audience").Value),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim("AccountId", account.AccountId.ToString()),
                new Claim(ClaimTypes.UserData, account.UserName.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                (_configuration.GetSection("JwtSecurityToken:Key").Value));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                        issuer: _configuration.GetSection("JwtSecurityToken:Issuer").Value,
                        audience: _configuration.GetSection("JwtSecurityToken:Audience").Value,
                        claims: claims,
                        expires: expirationUtc,
                        signingCredentials: signIn);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}