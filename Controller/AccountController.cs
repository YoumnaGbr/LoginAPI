using LoginAPI.Models;
using LoginAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LoginAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private static readonly Dictionary<string, string> UserOtps = new Dictionary<string, string>();

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // Check if the email is already in use
            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
            {
                ModelState.AddModelError("Email", "This email is already in use.");
                return BadRequest(ModelState);
            }

            // Check if the username is already in use
            var userNameExists = await _userManager.FindByNameAsync(model.UserName);
            if (userNameExists != null)
            {
                ModelState.AddModelError("UserName", "This username is already in use.");
                return BadRequest(ModelState);
            }

            var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var userName = user.UserName;

                var otp = GenerateOTP();

                UserOtps[model.Email] = otp;

                var subject = "Verify your email";
                #region HTMLForMail
                var htmlContent = @"
                      <!DOCTYPE html>
                      <html>
                      <head>
                          <style>
                              .container {
                                  font-family: Arial, sans-serif;
                                  max-width: 600px;
                                  margin: auto;
                                  padding: 20px;
                                  border: 1px solid #dcdcdc;
                                  border-radius: 5px;
                                  background-color: #f9f9f9;
                              }
                              .header {
                                  text-align: center;
                                  padding-bottom: 20px;
                              }
                              .otp {
                                  font-size: 24px;
                                  font-weight: bold;
                                  color: #2c3e50;
                                  text-align: center;
                                  margin: 20px 0;
                              }
                              .message {
                                  text-align: center;
                                  margin: 20px 0;
                              }
                              .footer {
                                  text-align: center;
                                  font-size: 12px;
                                  color: #888;
                                  margin-top: 30px;
                              }
                          </style>
                      </head>
                      <body>
                          <div class=""container"">
                              <div class=""header"">
                                  <h2>Verify Your Email</h2>
                              </div>
                              <p>Hi "+userName+@" </p>
                              <p>We received a request to verify your email address. Please use the OTP code below to verify your email:</p>
                              <div class=""otp"">" + otp + @"</div>
                              <div class=""message"">
                                  If you did not request this, please ignore this email.
                              </div>
                              <div class=""footer"">
                                  <p>Thank you,<br/>The LoginApp Team</p>
                              </div>
                          </div>
                      </body>
                      </html>
                      "; 
                #endregion

                //var message = $"Hi {model.UserName} : {otp}";

                await _emailSender.SendEmailAsync(model.Email, subject, htmlContent);

                return Ok("Please check your mail to verify it <3");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return Ok("Login successful");
            }

            return Unauthorized("Invalid login attempt");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Invalid email");

            // Generate a random OTP and store it in a dictionary for demo purposes
            var otp = new Random().Next(100000, 999999).ToString();
            UserOtps[model.Email] = otp; // Store the OTP (use a more secure storage in production)

            var subject = "Password Reset Request";
            var message = $"Your OTP code is: {otp}. Please use this code to reset your password.";

            await _emailSender.SendEmailAsync(user.Email, subject, message);

            return Ok("Please check your email for the OTP to reset your password.");
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Invalid email");

            // Check if the OTP matches
            if (!UserOtps.TryGetValue(model.Email, out var storedOtp) || storedOtp != model.Otp)
            {
                return BadRequest("Invalid or expired OTP.");
            }

            // Generate password reset token (hidden from the user)
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Reset the password using the token
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            // Remove OTP after usage
            UserOtps.Remove(model.Email);

            return result.Succeeded ? Ok("Password has been reset successfully.") : BadRequest("Error resetting password.");
        }


        private string GenerateOTP()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the OTP exists and matches
            if (UserOtps.TryGetValue(model.Email, out var storedOtp) && storedOtp == model.Otp)
            {
                UserOtps.Remove(model.Email); 
                return Ok("OTP verified successfully.");
            }

            return BadRequest("Invalid or expired OTP.");
        }
    }
   
}
