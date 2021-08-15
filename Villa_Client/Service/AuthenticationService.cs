using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Common;
using Microsoft.AspNetCore.Components.Authorization;
using Models;
using Newtonsoft.Json;
using Villa_Client.Service.IService;

namespace Villa_Client.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthenticationService(HttpClient client, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _client = client;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();

        }


        public async Task<RegistrationResponseDto> RegisterUser(UserRequestDto userForRegistration)
        {
            var content = JsonConvert.SerializeObject(userForRegistration);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/account/signUp", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RegistrationResponseDto>(contentTemp);

            if (response.IsSuccessStatusCode)
            {
                return new RegistrationResponseDto {IsRegistrationSuccessful = true};
            }
            else
            {
                return result;
            }
        }

        public async Task<AuthenticationResponseDto> Login(AuthenticationDto userFromAuthentication)
        {
            var content = JsonConvert.SerializeObject(userFromAuthentication);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/account/signIn", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AuthenticationResponseDto>(contentTemp);

            if (response.IsSuccessStatusCode)
            {
                await _localStorage.SetItemAsync(SD.Local_Token, result.Token);
                await _localStorage.SetItemAsync(SD.Local_UserDetails, result.UserDto);
                ((AuthStateProvider)_authStateProvider).NotifyUserLoggedIn(result.Token);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
                return new AuthenticationResponseDto{IsAuthSuccessful = true};
            }
            else
            {
                return result;
            }
        }

        public async Task LogOut()
        {
            await _localStorage.RemoveItemAsync(SD.Local_Token);
            await _localStorage.RemoveItemAsync(SD.Local_UserDetails);

            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
