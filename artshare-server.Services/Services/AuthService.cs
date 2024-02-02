using artshare_server.Contracts.DTOs;
using artshare_server.Core.Models;
using artshare_server.Services.CustomExceptions;
using artshare_server.Services.Interfaces;
using AutoMapper;
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
                throw new NullReferenceException("Wrong email or password.");
            }
            return "Login successfully.";
        }

        public async Task<bool> RegisterAsync(RegisterDTO registerData)
        {
            try
            {
                var account = await _accountService.GetAccountByEmailAsync(registerData.Email);
                if (account != null)
                {
                    throw new RegistrationException("Duplicate email.");
                }
                account = _mapper.Map<Account>(registerData);
                account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerData.Password);
                var result = await _accountService.CreateAccountAsync(account);
                return result;
            }
            catch (DbUpdateException ex)
            {
                throw new RegistrationException(ex.Message);
            }
        }

        private string CreateToken(Account account)
        {
            var nowUtc = DateTime.UtcNow;
            var expirationDuration =
                TimeSpan.FromMinutes(10);
            var expirationUtc = nowUtc.Add(expirationDuration);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration.GetSection("JwtSecurityToken:Subject").Value),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                (_configuration.GetSection("JwtSecurityToken:Key").Value));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                        issuer: _configuration.GetSection("JwtSecurityToken:Issuer").Value,
                        audience: _configuration.GetSection("JwtSecurityToken:JWTAuthClient").Value,
                        claims: claims,
                        expires: expirationUtc,
                        signingCredentials: signIn);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}