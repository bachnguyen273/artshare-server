﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace artshare_server.Contracts.DTOs
{
    public class RegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
		public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public string? Role { get; set; }
    }
}
