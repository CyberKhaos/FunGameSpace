using Microsoft.AspNetCore.Mvc;
using Models.Account.Dtos;
using Models.Apis;
using UserApi.Services.Interfaces;

namespace UserApi.Controllers.v1;

[ApiController]
[Route("user/v1/[controller]")]
public class AccountController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
    {
        return await ProcessResult(userService.RegisterAsync(registerUserDto.Username, registerUserDto.Password,
            registerUserDto.Email));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto loginUserDto)
    {
        return await ProcessResult(userService.LoginAsync(loginUserDto.Username, loginUserDto.Password));
    }

    [HttpDelete("delete-user/{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        return await ProcessResult(userService.DeleteUserAsync(id));
    }

    [HttpPut("change-user/{id:guid}")]
    public async Task<IActionResult> ChangeUser(Guid id, [FromBody] RegisterUserDto registerUserDto)
    {
        return await ProcessResult(userService.ChangeUserDataAsync(id, registerUserDto.Username, registerUserDto.Password,
            registerUserDto.Email));
    }
    
    [HttpGet("get-user/{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        return await ProcessResult(userService.GetUserAsync(id));
    }
    
    [HttpGet("get-users")]
    public async Task<IActionResult> GetUsers()
    {
        return await ProcessResult(userService.GetUsersAsync());
    }
    
    [HttpGet("check-username/{username}")]
    public async Task<IActionResult> CheckUsername(string username)
    {
        return await ProcessResult(userService.CheckUsernameAsync(username));
    }
    
    [HttpGet("check-email/{email}")]
    public async Task<IActionResult> CheckEmail(string email)
    {
        return await ProcessResult(userService.CheckEmailAsync(email));
    }
    
    private async Task<IActionResult> ProcessResult<T>(Task<ResponseModel<T>> task)
    {
        try
        {
            var response = await task;
            if (!response.Success)
                return BadRequest(response.Message);
            return Ok(response);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
    
    private async Task<IActionResult> ProcessResult(Task<ResponseModelBase> task)
    {
        try
        {
            var response = await task;
            if (!response.Success)
                return BadRequest(response.Message);
            return Ok(response);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}