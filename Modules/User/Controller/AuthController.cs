using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myapp.Modules.DTOs;
using myapp.Modules.User.Models;
using myapp.Modules.User.Interface;
using myapp.Modules.User.DTOs;
using Microsoft.EntityFrameworkCore;



namespace myapp.Modules.User.Controller
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase 
    {
        private readonly UserManager<AppUser> _usermanager;
        private readonly IOTP _otpservice;
        private readonly ITokenService _tokenService;
        public AuthController(UserManager<AppUser> usermanager, IOTP otpservice, ITokenService tokenService)
        {
            _usermanager = usermanager;
            _otpservice = otpservice;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                // Check if the model state is valid
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if the email is already taken
                var existingUser = await _usermanager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return BadRequest("Email is already registered.");
                }

                // Generate OTP code
                var otpcode = await _otpservice.GenerateOtpCodeAsync();

                var user = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    OtpCode = otpcode,
                    OtpExpiresAt = DateTime.UtcNow.AddMinutes(5),
                    IsVerified = false,
                };

                // Create the user
                var createUser = await _usermanager.CreateAsync(user, registerDto.ConfirmPassword);
                if (createUser.Succeeded)
                {
                    // Send OTP email
                    await _otpservice.SendOtpEmailAsync(user.Email, otpcode);
                    return Ok("User created successfully, OTP sent to email.");
                }

                // If user creation failed, return the errors
                var errors = string.Join(", ", createUser.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }

            catch (Exception ex)
            {

                // Return a generic error response
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred. Please try again later.",
                    exceptionMessage = "Internal Server Error",
                    stackTrace = ex.StackTrace // Consider removing stack trace in production
                });
            }
        
        }
        [HttpPost("otp_verify")]
        public async Task<IActionResult> VerifyCode ([FromBody] OTPCode otpCode)
        {
            // Check if the request body is invalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            // Extract OTP
            string userOtp = otpCode.OtpCode;

            var user = await _usermanager.Users.FirstOrDefaultAsync(u => u.OtpCode == userOtp);

            // Check if user exists
            if (user == null)
            {
                return BadRequest(new { message = "User not found for the provided OTP." });
            }

            bool isVerified = await _otpservice.VerifyOTP(userOtp);

            if (!isVerified)
            {
                return BadRequest(new { message = "Invalid or expired OTP." });
            }

            var token = _tokenService.CreateToken(user);

            return Ok(new { token });
        }
    }
}
