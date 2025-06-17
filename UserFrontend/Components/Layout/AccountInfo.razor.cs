using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Models.Account;

namespace UserFrontend.Components.Layout;

public partial class AccountInfo(NavigationManager navigationManager, ISessionStorageService sessionStorage) : ComponentBase
{
    private bool isLoggedIn = false;
    private bool isDropdownOpen = false;
    private UserInfo? userInfo = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var storedUserKey = await sessionStorage.GetItemAsync<string>("UserKey");
            if (!string.IsNullOrEmpty(storedUserKey))
            {
                isLoggedIn = true;
                userInfo = new UserInfo
                {
                    Id = Guid.Parse(storedUserKey),
                    Username = await sessionStorage.GetItemAsync<string>("UserName"),
                    Email = await sessionStorage.GetItemAsync<string>("UserEmail")
                };
                StateHasChanged();
            }
        }
    }
    
    private async Task Logout()
    {
        await sessionStorage.RemoveItemAsync("UserKey");
        await sessionStorage.RemoveItemAsync("UserName");
        await sessionStorage.RemoveItemAsync("UserEmail");
        isLoggedIn = false;
        userInfo = null;
        navigationManager.NavigateTo("account/login");
    }

    private Task Login()
    {
        navigationManager.NavigateTo("account/login");
        return Task.CompletedTask;
    }

    private Task Register()
    {
        navigationManager.NavigateTo("account/register");
        return Task.CompletedTask;
    }

    private void ToggleAccountDropdown()
    {
        isDropdownOpen = !isDropdownOpen;
    }
}