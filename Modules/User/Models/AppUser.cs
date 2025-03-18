using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace myapp.Modules.User.Models
{
    public class AppUser : IdentityUser
    {
        public string OtpCode { get; set; }
        public DateTime? OtpExpiresAt { get; set; }
        public bool IsVerified { get; set; } = false;
    }
}