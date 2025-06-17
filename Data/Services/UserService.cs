using System.Net.Http.Json;
using Blazored.SessionStorage;
using Models.Account;
using Models.Account.Dtos;
using Models.Apis;
using Services.Interfaces;

namespace Services;

public class UserService(HttpClient httpClient, ISessionStorageService sessionStorage) : IUserService
{
    public async Task<ResponseModel<UserInfo>> LoginUser(LoginUserDto loginUserDto)
    {
        var response = await httpClient.PostAsJsonAsync($"{ApiUrl.UserApi}/account/login", loginUserDto);
        if (!response.IsSuccessStatusCode)
            return new ResponseModel<UserInfo>()
            {
                Success = false,
                Message = "No message received from the server"
            };
        var responseModel = await response.Content.ReadFromJsonAsync<ResponseModel<UserInfo>>();
        if (responseModel is not { Success: true, Data: not null })
            return new ResponseModel<UserInfo>()
            {
                Success = false,
                Message = responseModel?.Message ?? "No message received from the server"
            };
        await sessionStorage.SetItemAsync("UserKey", responseModel.Data.Id.ToString());
        await sessionStorage.SetItemAsync("UserName", responseModel.Data.Username);
        await sessionStorage.SetItemAsync("UserEmail", responseModel.Data.Email);
        return new ResponseModel<UserInfo>()
        {
            Success = true,
            Message = "User logged in",
            Data = responseModel.Data
        };

    }

    public async Task<ResponseModel<UserInfo>> RegisterUser(RegisterUserDto registerUserDto)
    {
        var response = await httpClient.PostAsJsonAsync($"{ApiUrl.UserApi}/account/register", registerUserDto);
        if (!response.IsSuccessStatusCode)
            return new ResponseModel<UserInfo>()
            {
                Success = false,
                Message = "No message received from the server"
            };
        var responseModel = await response.Content.ReadFromJsonAsync<ResponseModel<UserInfo>>();
        if (responseModel is not { Success: true, Data: not null })
            return new ResponseModel<UserInfo>()
            {
                Success = false,
                Message = responseModel?.Message ?? "No message received from the server"
            };
        await sessionStorage.SetItemAsync("UserKey", responseModel.Data.Id.ToString());
        await sessionStorage.SetItemAsync("UserName", responseModel.Data.Username);
        await sessionStorage.SetItemAsync("UserEmail", responseModel.Data.Email);
        return new ResponseModel<UserInfo>()
        {
            Success = true,
            Message = "User registered",
            Data = responseModel.Data
        };
    }

    public async Task LogoutUser(Guid userId)
    {
        await sessionStorage.RemoveItemAsync("UserKey");
        await sessionStorage.RemoveItemAsync("UserName");
        await sessionStorage.RemoveItemAsync("UserEmail");
    }

    public Task<ResponseModelBase> DeleteUser(Guid userId)
    {
        var response = httpClient.DeleteAsync($"{ApiUrl.UserApi}/account/{userId}");
        if (!response.Result.IsSuccessStatusCode)
            return Task.FromResult(new ResponseModelBase()
            {
                Success = false,
                Message = "No message received from the server"
            });
        var responseModel = response.Result.Content.ReadFromJsonAsync<ResponseModelBase>().Result;
        return responseModel switch
        {
            null => Task.FromResult(new ResponseModelBase()
            {
                Success = false, Message = "No message received from the server"
            }),
            { Success: false } => Task.FromResult(new ResponseModelBase()
            {
                Success = false, Message = responseModel.Message
            }),
            _ => Task.FromResult(responseModel)
        };
    }

    public Task<UserInfo> UpdateUser(Guid userId, RegisterUserDto updateUserDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsUsernameAvailable(string username)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsEmailAvailable(string username)
    {
        throw new NotImplementedException();
    }

    public Task<UserInfo> GetUserInfo(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserInfo>> GetUserInfos()
    {
        throw new NotImplementedException();
    }
}