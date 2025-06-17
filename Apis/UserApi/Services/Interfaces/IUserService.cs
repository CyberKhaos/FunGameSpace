using Models.Account;
using Models.Apis;

namespace UserApi.Services.Interfaces;

public interface IUserService
{
    Task<ResponseModelBase> RegisterAsync(string username, string password, string email);
    Task<ResponseModel<UserInfo>> LoginAsync(string username, string password);
    Task<ResponseModelBase> DeleteUserAsync(Guid id);
    Task<ResponseModelBase> ChangeUserDataAsync(Guid id, string username, string password, string email);
    Task<ResponseModel<UserInfo>> CheckUsernameAsync(string username);
    Task<ResponseModel<UserInfo>> CheckEmailAsync(string email);
    Task<ResponseModel<UserInfo>> GetUserAsync(Guid id);
    Task<ResponseModel<List<UserInfo>>> GetUsersAsync();
}