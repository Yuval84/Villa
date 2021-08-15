using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Components;
using Models;
using Villa_Client.Service.IService;

namespace Villa_Client.Pages.Authentication
{
    public partial class Login
    {
        private readonly AuthenticationDto UserForAuthentication = new AuthenticationDto();
        public bool IsProcessing { get; set; } = false;
        public bool ShowAuthenticationErrors { get; set; }
        public string Errors { get; set; }
        public string ReturnUrl { get; set; }


        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }


        private async Task LoginUser()
        {
            ShowAuthenticationErrors = false;
            IsProcessing = true;
            var result = await AuthenticationService.Login(UserForAuthentication);
            if (result.IsAuthSuccessful)
            {
                IsProcessing = false;
                var absoluteUri = new Uri(NavigationManager.Uri);
                var queryParam = HttpUtility.ParseQueryString(absoluteUri.Query);
                ReturnUrl = queryParam["returnUrl"];
                if (string.IsNullOrEmpty(ReturnUrl))
                {
                    NavigationManager.NavigateTo("/");
                }
                NavigationManager.NavigateTo("/"+ReturnUrl);
            }
            else
            {
                IsProcessing = false;
                Errors = result.ErrorMessage;
                ShowAuthenticationErrors = true;
            }
        }
    }
}

