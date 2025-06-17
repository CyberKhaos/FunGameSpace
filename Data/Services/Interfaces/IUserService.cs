using Models.Account;
using Models.Account.Dtos;
using Models.Apis;

namespace Services.Interfaces;

public interface IUserService
{
    Task<ResponseModel<UserInfo>> LoginUser(LoginUserDto loginUserDto);
    Task<ResponseModel<UserInfo>> RegisterUser(RegisterUserDto registerUserDto);
    Task LogoutUser(Guid userId);
    Task<ResponseModelBase> DeleteUser(Guid userId);
    Task<UserInfo> UpdateUser(Guid userId, RegisterUserDto updateUserDto);
    Task<bool> IsUsernameAvailable(string username);
    Task<bool> IsEmailAvailable(string username);
    Task<UserInfo> GetUserInfo(Guid userId);
    Task<List<UserInfo>> GetUserInfos();
}