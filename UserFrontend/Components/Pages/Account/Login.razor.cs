using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Models.Account;
using Models.Account.Dtos;
using Models.Apis;

namespace UserFrontend.Components.Pages.Account;

public partial class Login(HttpClient httpClient, ISessionStorageService sessionStorage, NavigationManager navigationManager) : ComponentBase
{
    private LoginUserDto _loginUserDto = new();
    private string _errorMessage = string.Empty;

    private async Task LoginUser()
    {
        var response = await httpClient.PostAsJsonAsync($"{ApiUrl.UserApi}/account/login", _loginUserDto);
        if (response.IsSuccessStatusCode)
        {
            var responseModel = await response.Content.ReadFromJsonAsync<ResponseModel<UserInfo>>();
            if (responseModel is { Success: true, Data: not null})
            {
                await sessionStorage.SetItemAsync("UserKey", responseModel.Data.Id.ToString());
                await sessionStorage.SetItemAsync("UserName", responseModel.Data.Username);
                await sessionStorage.SetItemAsync("UserEmail", responseModel.Data.Email);
                navigationManager.NavigateTo("/");
                navigationManager.Refresh(forceReload: true);
            }
            else
            {
                _errorMessage = responseModel?.Message ?? "No message received from the server";
            }
        }
        _errorMessage = "No message received from the server";
    }
}