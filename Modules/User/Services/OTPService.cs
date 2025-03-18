using Microsoft.AspNetCore.Identity;
using myapp.Modules.User.Models;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using myapp.Modules.User.Interface;

namespace myapp.Modules.User.Services
{
    public class OTPService : IOTP
    {
        private readonly UserManager<AppUser> _userManager;
        public OTPService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public Task<string> GenerateOtpCodeAsync()
        {
            // Generate a 6-digit OTP code
            Random rand = new Random();
            string otpCode = rand.Next(100000, 999999).ToString();
            return Task.FromResult(otpCode); // Wrap the result in a Task
        }

        // Generate a 6-digit OTP code

        public async Task SaveOtpToDatabaseAsync(string otpCode, int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (userId != null)
            {
                user.OtpCode = otpCode;
                user.OtpExpiresAt = DateTime.UtcNow.AddMinutes(5); // OTP expires after 5 minutes
                await _userManager.UpdateAsync(user);
            }

        }

        public async Task SendOtpEmailAsync(string email, string otpCode)
        {
            // Create an email message
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Khun Myat Mg", "khunmyatmg007@gmail.com.com")); // Replace with your app's email
            emailMessage.To.Add(new MailboxAddress("", email)); // Recipient's email
            emailMessage.Subject = "Your OTP Code"; // Email subject

            // Email body (message content)
            string message = $"Your OTP code is: {otpCode}";
            emailMessage.Body = new TextPart("plain") { Text = message };

           // Set up the SMTP client and send the email
            using (var smtpClient = new SmtpClient())
            {
                try
                {
                    // Connect to Gmail's SMTP server
                    await smtpClient.ConnectAsync("smtp.gmail.com", 587, false);

                    // Authenticate with your Gmail address and App Password
                    await smtpClient.AuthenticateAsync("khunmyatmg007@gmail.com", "dzwo tqpd ivcq gkla");

                    // Send the email
                    await smtpClient.SendAsync(emailMessage);

                    // Disconnect from the SMTP server
                    await smtpClient.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., log them)
                    throw new Exception("Error sending OTP email", ex);
                }
            }
        }

        public async Task<bool> VerifyOTP(string otpCode)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.OtpCode == otpCode);

            if (user == null || user.OtpExpiresAt < DateTime.UtcNow)
            {
                return false; // OTP is invalid or expired
            }

            user.IsVerified = true;
            user.OtpCode = null;
            user.OtpExpiresAt = null;
            await _userManager.UpdateAsync(user);

            return true; // OTP verification successful
        }
    }
}