using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myapp.Modules.User.Interface
{
    public interface IOTP
    {
        Task<string> GenerateOtpCodeAsync();
        Task SaveOtpToDatabaseAsync(string otpCode, int userId);
        Task SendOtpEmailAsync(string email, string otpCode);

        Task <bool>VerifyOTP(string otpCode);
    }
}