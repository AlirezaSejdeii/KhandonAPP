using Blazored.LocalStorage;
using Khandon.Shared.Dto.Base.User;
using Khandon.Shared.Dto.Request.Auth;
using Khandon.Shared.Dto.Request.User;
using Khandon.Shared.Dto.Response.User;
using Khandon.SharerdKernel.UI.Applications.IServices;
using Khandon.SharerdKernel.UI.Applications.Services;
using Khandon.SharerdKernel.UI.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Khandon.SharerdKernel.UI.State
{
    public delegate Task UserStateChange();
    public class UserStateService : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;
        private readonly NavigationManager navigationManger;
        private readonly IHttpService httpService;
        private readonly ISnackbar snackbar;
        private readonly BookStateService bookStateService;
        //private readonly IUserState userStateService;

        public event UserStateChange OnUserstateChange;

        string LoginPage = "/Account/Login";

        private string TokenKey = "_Khandon_auth_token";
        private UserState state;
        public UserStateService(ILocalStorageService localStorageService, NavigationManager navigationManger, ISnackbar snackbar, IHttpService httpService, BookStateService bookStateService)
        {
            state = new();
            this.localStorageService = localStorageService;
            this.navigationManger = navigationManger;
            this.snackbar = snackbar;
            this.httpService = httpService;
            this.bookStateService = bookStateService;
        }

        //SignUp
        public async Task SignUpUserAsync(UserSignUp userSignUp)
        {
            if (userSignUp == null) return;

            //Call Api
            try
            {
                var result = await httpService.Post<UserSignUp, AuthResult>($"{ApplicationConfig.ApiUrl}/V1/Users/SignUpUser", userSignUp);

                if (result.Response.Messages.Any() && result.Response.Result == false)
                {
                    snackbar.Add(result.Response.Messages.FirstOrDefault(), Severity.Error);
                }
                else if (result.Response.Result == false)
                {
                    snackbar.Add("مشکلی در ایجاد حساب کاربری وجود دارد", Severity.Error);
                }
                if (result.Response.Messages.Any() && result.Response.Result == true)
                {
                    snackbar.Add(result.Response.Messages.FirstOrDefault(), Severity.Success);
                }
                else if (result.Response.Result == true)
                {
                    snackbar.Add("حساب کاربری شما با موفقیت ایجاد شد، جهت تایید حساب کاربری به ایمیل خود مراجعه کنید", Severity.Success);
                }
                if (result.Response.Result == true)
                {
                    navigationManger.NavigateTo("/");
                }
            }
            catch (Exception e)
            {
                snackbar.Add(ApplicationConfig.ApplicationIsOffline, Severity.Warning);
            }

        }
        //SignUp Confirm
        public async Task<bool> SignUpConfirmAsync(string token, string email)
        {
            Console.WriteLine($"Email is: {email} \n Token is: {token}");

            //token = System.Web.HttpUtility.UrlEncode(token);

            string url = $"{ApplicationConfig.ApiUrl}/V1/Users/ConfirmAccount";
            url = QueryHelpers.AddQueryString(url, "Email", email);
            url = QueryHelpers.AddQueryString(url, "Token", token);

            //url = System.Web.HttpUtility.UrlDecode(url);

            Uri f = new Uri(url);
            var result = await httpService.Get<AuthResult>(url);

            return result.Response.Result;
        }
        //Login
        public async Task LoginUserAsync(UserLogin userLogin)
        {
            if (userLogin == null) return;

            //Call Api
            try
            {
                var result = await httpService.Post<UserLogin, AuthResult>($"{ApplicationConfig.ApiUrl}/V1/Users/LoginInUser", userLogin);

                if (result.Response.Result == true && result.Response.Token != null)
                {
                    if (result.Response.Token.ExpireDateUtc >= DateTime.UtcNow)
                    {
                        //Store token
                        state = new();

                        state.Username = userLogin.UsernameOrEmail;
                        state.tokenDto = result.Response.Token;

                        await StoreToken(state);
                        snackbar.Add("ورود با موفقیت انجام شد", Severity.Success);
                        await OnUserstateChange();
                        navigationManger.NavigateTo("/");
                    }
                }
                else
                {
                    foreach (var item in result.Response.Messages)
                    {
                        snackbar.Add(item, Severity.Warning);
                    }
                }
            }
            catch (Exception e)
            {
                snackbar.Add(ApplicationConfig.ApplicationIsOffline, Severity.Error);
            }

        }
        public async Task<UserState> GetUserState()
        {
            if (state.tokenDto != null)
            {
                if (state.tokenDto.ExpireDateUtc >= DateTime.UtcNow)
                    return state;
                else
                    return null;
            }
            else
            {
                var result = await localStorageService.GetItemAsync<UserState>(TokenKey);
                if (result == null)
                    return null;
                else
                {

                    if (result.tokenDto.ExpireDateUtc >= DateTime.UtcNow)
                        return result;
                    else
                        return null;
                }
            }
        }
        private async Task StoreToken(UserState userState)
        {
            await localStorageService.SetItemAsync<UserState>(TokenKey, userState);

            await GetAuthenticationStateAsync();


        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            state = await GetUserState();

            if (state == null || state.tokenDto == null)
            {
                //whaite list
                if (!navigationManger.Uri.Contains("Account"))
                {
                    navigationManger.NavigateTo(LoginPage);
                }

                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, state.Username));

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "jwt");



            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            AuthenticationState AppState = new AuthenticationState(claimsPrincipal);
            NotifyAuthenticationStateChanged(Task.FromResult(AppState));
            return AppState;
        }
        //RecoveryPassword
        public async Task RecoveryPassword(RecoveryPasswordModel recoveryPasswordModel)
        {
            try
            {
                var result = await httpService.Post<RecoveryPasswordModel, AuthResult>($"{ApplicationConfig.ApiUrl}/V1/Users/RecoveryPassword", recoveryPasswordModel);
                if (result.Response.Result == true)
                {
                    if (result.Response.Messages.Any())
                    {
                        foreach (var item in result.Response.Messages)
                        {
                            snackbar.Add(item, Severity.Success);
                        }
                    }
                    else
                    {
                        snackbar.Add("بزودی لینکی برای تعغیر کلمه عبور برای شما ارسال خواهد شد");
                    }
                    navigationManger.NavigateTo(LoginPage);
                }
                else
                {
                    if (result.Response.Messages.Any())
                    {
                        foreach (var item in result.Response.Messages)
                        {
                            snackbar.Add(item, Severity.Success);
                        }
                    }
                    else
                    {
                        snackbar.Add("مشکلی در بازیابی کلمه عبور وجود دارد");
                    }
                }
                await OnUserstateChange();
            }
            catch (Exception e)
            {
                snackbar.Add(ApplicationConfig.ApplicationIsOffline, Severity.Error);
            }
        }
        //RecoveryPasswordConfrim
        public async Task RecoveryPasswordConfirm(RecoveryPasswordConfirmViewModel confirmViewModel)
        {
            if (confirmViewModel == null) return;
            try
            {
                var result = await httpService.Post<RecoveryPasswordConfirmViewModel, AuthResult>($"{ApplicationConfig.ApiUrl}/V1/Users/RecoveryPasswordConfirm", confirmViewModel);
                if (result.Response.Result == true)
                {
                    if (result.Response.Messages.Any())
                    {
                        foreach (var item in result.Response.Messages)
                        {
                            snackbar.Add(item, Severity.Success);
                        }
                    }
                    else
                    {
                        snackbar.Add("کلمه عبور تعغیر یافت");
                    }
                    navigationManger.NavigateTo(LoginPage);
                }
                else
                {
                    if (result.Response.Messages.Any())
                    {
                        foreach (var item in result.Response.Messages)
                        {
                            snackbar.Add(item, Severity.Warning);
                        }
                    }
                    else
                    {
                        snackbar.Add("مشکلی در بازیابی کلمه عبور وجود دارد");
                    }
                }
                await OnUserstateChange();
            }
            catch (Exception e)
            {
                snackbar.Add(ApplicationConfig.ApplicationIsOffline, Severity.Error);
            }
        }
        //Rest Password
        public async Task RestPassword(RestPasswordViewModel restPassword)
        {
            if (restPassword == null) return;
            try
            {
                var result = await httpService.Post<RestPasswordViewModel, AuthResult>($"{ApplicationConfig.ApiUrl}/V1/Users/RestPasssword", restPassword);

                if (result.Response.Result == true)
                {
                    snackbar.Add("رمز عبور با موفقیت عوض شد، با رمز جدید وارد شوید", Severity.Success);
                    await LogOutUser();
                }
                await OnUserstateChange();
            }
            catch (Exception e)
            {
                snackbar.Add(ApplicationConfig.ApplicationIsOffline, Severity.Error);
            }
        }
        //Logout 
        public async Task LogOutUser()
        {
            await localStorageService.RemoveItemAsync(TokenKey);
            state = new();
            bookStateService.ClearState();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new(new ClaimsPrincipal()))));
            await OnUserstateChange();
            navigationManger.NavigateTo(LoginPage);
        }
    }
}
