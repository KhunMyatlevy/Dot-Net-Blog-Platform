using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace myapp.Modules.User.DTOs
{
    public class OTPCode
    {
        [Required(ErrorMessage = "OTP code is required.")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP code must be exactly 6 digits.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "OTP code must contain only numbers.")]
        public string OtpCode { get; set; }
    }
}