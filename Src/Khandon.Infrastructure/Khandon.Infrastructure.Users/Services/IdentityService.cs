using Khandon.Core.Interfaces.User.Identity;
using Khandon.Infrastructure.Users.Models.User;
using Khandon.Shared.Dto.Request.Auth;
using Khandon.Shared.Dto.Request.User;
using Khandon.Shared.Dto.Response.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Khandon.Infrastructure.Users.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService tokenService;
        private readonly IEmailService emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration configuration;
        public IdentityService(UserManager<ApplicationUser> userManager, ITokenService tokenService, SignInManager<ApplicationUser> signInManager, IEmailService emailService, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _userManager = userManager;
            this.tokenService = tokenService;
            _signInManager = signInManager;
            this.emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
        }

        //SignUp
        public async Task<AuthResult> UserSignUpAsync(UserSignUp userSignUp)
        {
            bool AnyUser = _userManager.Users.Any(a => a.Email == userSignUp.Email || a.UserName == userSignUp.Username);
            if (AnyUser)
            {
                return new AuthResult()
                {
                    Result = false,
                    Token = null,
                    Messages = new()
                    {
                        "کاربری با این مشخصات وجود دارد"
                    }
                };
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = userSignUp.Email,
                UserName = userSignUp.Username,
                CreateDateUtc = DateTime.UtcNow
            };
            var result = await _userManager.CreateAsync(user, userSignUp.Password);

            if (result.Succeeded)
            {
                await SendConfirm(user);

                return new AuthResult()
                {
                    Result = true,
                    Messages = new() { "لینکی برای تایید حساب کاربری به ایمیل شما ارسال شده" }
                };
            }
            else
            {
                return new AuthResult()
                {
                    Messages = result.Errors.Select(a => a.Description).ToList()
                };
            }
        }
        //Login
        public async Task<AuthResult> UserLoginAsync(UserLogin userLogin)
        {
            ApplicationUser user = await GetUserAsync(userLogin.UsernameOrEmail);


            if (user == null)
            {
                return new AuthResult()
                {
                    Messages = new(){
                        "کاربری با این مشخصات وجود ندارد"
                    }
                };
            }
            var result = await _signInManager.PasswordSignInAsync(user, userLogin.Password, false, true);

            if (result.IsLockedOut)
            {
                return new AuthResult()
                {
                    Messages = new(){
                        "به علت مراجعه زیاد حساب کاربری شما تا دقایقی در حالت تعلیق است"
                    }
                };
            }
            if (user.EmailConfirmed == false)
            {
                await SendConfirm(user);
                return new AuthResult()
                {
                    Messages = new(){
                        "ایمیل شما تایید نشده، لینکی برای تایید حساب کاربری به ایمیل شما ارسال شد."
                    }
                };
            }
            if (result.IsNotAllowed)
            {
                return new AuthResult()
                {
                    Messages = new(){
                        "دسترسی به این حساب کاربری امکان پذیر نیست"
                    }
                };
            }
            if (result.Succeeded)
            {
                TokenDto token = tokenService.GenrateToken(user);
                return new()
                {
                    Result = true,
                    Token = token
                };
            }
            else
            {
                return new()
                {
                    Messages = new()
                    {
                        "حساب کاربری وجود ندارد"
                    }
                };
            }
        }
        async Task<bool> SendConfirm(ApplicationUser user)
        {
            //genrate token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = System.Web.HttpUtility.UrlEncode(token);
            //genrate backlink
            var confirmationLink = $"{configuration.GetSection("ConfirmationEmailClientAddress").Value}?token={token}&&email={user.Email}";

            //HTML template ViewModel
            string body = $"<a href='{confirmationLink}'>کلیک کنید</a>";

            bool IsSended = emailService.SendMail(user.UserName, "ایمیل تایید حساب کاربری", user.Email, body);

            return await Task.FromResult(IsSended);
        }

        public async Task<AuthResult> ConfirmEmailAsync(string token, string email)
        {
            //token = System.Web.HttpUtility.UrlDecode(token);

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new()
                {
                    Result = false,
                    Messages = new()
                    {
                        "کاربر یافت نشد"
                    }
                };
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return new AuthResult()
                {
                    Result = true,
                    Messages = new()
                    {
                        "تایید حساب کاربری با موفقیت انجام شد، ممنون از همکاری شما"
                    }
                };
            }
            return new AuthResult()
            {
                Messages = result.Errors.Select(a => a.Description).ToList()
            };
        }


        public async Task<AuthResult> RestPasswordAsync(RestPasswordViewModel restPassword, string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new() { Result = false, Messages = new() { "کاربری با این ایمیل و نام کاربری یافت نشد" } };
            }
            var result = await _userManager.ChangePasswordAsync(user, restPassword.OldPassword, restPassword.NewPassword);
            if (result.Succeeded)
            {
                return new() { Result = true, Messages = new() { "کلمه عبور شما با موفقیت تعغیر یافت" } };
            }
            else
            {
                return new() { Result = false, Messages = result.Errors.Select(a => a.Description).ToList() };
            }
        }

        public async Task<AuthResult> RecoveryPasswordAsync(string usernameOrEmail)
        {
            ApplicationUser user = await GetUserAsync(usernameOrEmail);
            if (user == null)
            {
                return new AuthResult() { Messages = new() { "کاربری با این مشخصات یافت نشد" } };
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
          
            token = System.Web.HttpUtility.UrlEncode(token);

            //genrate backlink
            var confirmationLink = $"{configuration.GetSection("RecoveryPasswordClientAddress").Value}?token={token}&&email={user.Email}";

            //HTML template ViewModel
            string body = $" <h4>تعغیر کلمه عبور</h4><br><a href='{confirmationLink}'>کلیک کنید</a>";

            var result = emailService.SendMail(user.UserName, "تعغیر کلمه عبور", user.Email, body);
            if (result == true)
            {
                return new AuthResult()
                {
                    Messages = new() { "ایمیلی حاوی لینک تعغیر کلمه عبور برای شما شد" },
                    Result = true
                };
            }
            return new AuthResult() { Messages = new() { "تعغیر کلمه عبور فعلا امکان پذیر نیست، در زمان های اینده دوباره تلاش کنید" } };
        }

        public async Task<AuthResult> RecoveryPasswordConfirmAsync(RecoveryPasswordConfirmViewModel recovery)
        {
            ApplicationUser user = await GetUserAsync(recovery.EmailOrUsername);
            if (user == null)
            {
                return new() { Result = false, Messages = new() { "کاربری با این ایمیل و نام کاربری یافت نشد" } };
            }
            //recovery.Token= System.Net.WebUtility.UrlEncode(recovery.Token);

            var result = await _userManager.ResetPasswordAsync(user: user, token: recovery.Token, newPassword: recovery.NewPassword);
            if (result.Succeeded)
            {
                return new()
                {
                    Messages = new()
                    {
                        "عملیات با موفقیت انجام شد"
                    },
                    Result = true
                };
            }
            else
            {
                return new() { Messages = result.Errors.Select(a => a.Description).ToList() };
            }

        }

        private async Task<ApplicationUser> GetUserAsync(string UsernameOrEmail)
        {
            ApplicationUser user = new();
            var userByEmail = await _userManager.FindByEmailAsync(UsernameOrEmail);
            if (userByEmail != null)
            {
                user = userByEmail;
            }
            else
            {
                var userByName = await _userManager.FindByNameAsync(UsernameOrEmail);
                user = userByName;
            }
            return user;
        }
    }
}

