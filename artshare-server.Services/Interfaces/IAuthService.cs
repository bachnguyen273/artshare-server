﻿using artshare_server.Contracts.DTOs;
using artshare_server.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDTO loginData);
        Task<bool> RegisterAsync(AccountRole accountRole, RegisterDTO registerData);
    }
}
