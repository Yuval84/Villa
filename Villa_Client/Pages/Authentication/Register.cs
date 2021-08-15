using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Models;
using Villa_Client.Service;
using Villa_Client.Service.IService;

namespace Villa_Client.Pages.Authentication
{
    public partial class Register
    {
        private readonly UserRequestDto UserForRegistration = new UserRequestDto();
        public bool IsProcessing { get; set; } = false;
        public bool ShowRegistrationErrors { get; set; }
        public IEnumerable<string> Errors { get; set; }

        
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }


        private async Task RegisterUser()
        {
            ShowRegistrationErrors = false;
            IsProcessing = true;
            var result = await AuthenticationService.RegisterUser(UserForRegistration);
            if (result.IsRegistrationSuccessful)
            {
                IsProcessing = false;
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                IsProcessing = false;
                Errors = result.Errors;
                ShowRegistrationErrors = true;
            }
        }
    }
}
