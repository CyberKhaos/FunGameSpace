using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Models.Account;
using Models.Apis;
using UserApi.Data;
using UserApi.Services.Interfaces;

namespace UserApi.Services;

public class UserService(DataBaseContext dbContext) : IUserService
{
    /// <summary>
    /// Create a new User
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<ResponseModelBase> RegisterAsync(string username, string email, string password)
    {
        var check = await CheckEmailAsync(email);
        if (!check.Success)
            return new ResponseModelBase()
            {
                Success = false,
                Message = "Email already exists"
            };
        check = await CheckUsernameAsync(username);
        if (!check.Success)
            return new ResponseModelBase()
            {
                Success = false,
                Message = "Username already exists"
            };
        var user = new Models.Account.User
        {
            Id = Guid.NewGuid(),
            Name = username,
            Email = email,
            PasswordHashed = HashString(password),
            EmailHashed = HashString(email)
        };
        
        await dbContext.User.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return new ResponseModelBase()
        {
            Success = true,
            Message = "User created"
        };
    }

    /// <summary>
    /// Check if the user exists and the password is correct
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<ResponseModel<UserInfo>> LoginAsync(string username, string password)
    {
        var user = await dbContext.User.FirstOrDefaultAsync(x => x.Name == username);
        if (user is null)
            return new ResponseModel<UserInfo>()
            {
                Success = false,
                Message = "User not found"
            };
        if (user.PasswordHashed != HashString(password))
            return new ResponseModel<UserInfo>()
            {
                Success = false,
                Message = "Password is incorrect"
            };

        return new ResponseModel<UserInfo>()
        {
            Success = true,
            Message = $"Login successful",
            Data = new UserInfo()
            {
                Id = user.Id,
                Username = user.Name,
                Email = user.Email
            }
        };
    }

    /// <summary>
    /// Delete a User by the ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResponseModelBase> DeleteUserAsync(Guid id)
    {
        var user = await dbContext.User.FirstOrDefaultAsync(x => x.Id == id);
        if (user is null)
            return new ResponseModelBase()
            {
                Success = false,
                Message = "User not found"
            };
        
        dbContext.User.Remove(user);
        await dbContext.SaveChangesAsync();
        
        if (await dbContext.User.FirstOrDefaultAsync(x => x.Id == id) is null)
            return new ResponseModelBase()
            {
                Success = true,
                Message = "User deleted"
            };

        return new ResponseModelBase()
        {
            Success = false,
            Message = "User not deleted"
        };
    }

    /// <summary>
    /// Change the user data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<ResponseModelBase> ChangeUserDataAsync(Guid id, string username, string password, string email)
    {
        var user = dbContext.User.FirstOrDefault(x => x.Id == id);
        if (user is null)
            return new ResponseModelBase()
            {
                Success = false,
                Message = "User not found"
            };
        
        user.Name = username;
        user.Email = email;
        user.PasswordHashed = HashString(password);
        user.EmailHashed = HashString(email);
        
        dbContext.Update(user);
        await dbContext.SaveChangesAsync();

        if (await dbContext.User.FirstOrDefaultAsync(x => x.Id == id) is null)
            return new ResponseModelBase()
            {
                Success = true,
                Message = "User updated"
            };
        
        return new ResponseModelBase()
        {
            Success = false,
            Message = "User not updated"
        };
    }

    /// <summary>
    /// Checks if the username is already in use
    /// </summary>
    /// <param name="username">The username to check</param>
    /// <returns>False if username already exists. True if username dose not exists</returns>
    public async Task<ResponseModel<UserInfo>> CheckUsernameAsync(string username)
    {
        var usernames = await dbContext.User.AnyAsync(x => x.Name == username);
        if (usernames)
            return new ResponseModel<UserInfo>()
            {
                Success = false,
                Message = "Username already exists"
            };
        return new ResponseModel<UserInfo>()
        {
            Success = true,
            Message = "Username dose not exists"
        };
    }

    /// <summary>
    /// Checks if the email is already in use
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<ResponseModel<UserInfo>> CheckEmailAsync(string email)
    {
        var emails = await dbContext.User.AnyAsync(x => x.Email == email);
        if (emails)
            return new ResponseModel<UserInfo>()
            {
                Success = false,
                Message = "Email already exists"
            };
        return new ResponseModel<UserInfo>()
        {
            Success = true,
            Message = "Email dose not exists"
        };
    }

    /// <summary>
    /// Find a user by the ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResponseModel<UserInfo>> GetUserAsync(Guid id)
    {
        var result = await dbContext.User.FirstOrDefaultAsync(x => x.Id == id);
        if (result is null)
            return new ResponseModel<UserInfo>()
            {
                Success = false,
                Message = "User not found"
            };
        return new ResponseModel<UserInfo>()
        { 
            Data = new UserInfo()
            {
            Id = result.Id,
            Username = result.Name,
            Email = result.Email 
            }
        };
    }

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns></returns>
    public async Task<ResponseModel<List<UserInfo>>> GetUsersAsync()
    {
        var result = await dbContext.User.ToListAsync();
        if (result.Count == 0)
            return new ResponseModel<List<UserInfo>>()
            {
                Success = false,
                Message = "No users found"
            };

        return new ResponseModel<List<UserInfo>>()
        {
            Success = true,
            Message = "",
            Data = result.Select(x => new UserInfo()
            {
                Id = x.Id,
                Username = x.Name,
                Email = x.Email
            }).ToList()
        };
    }

    /// <summary>
    /// Hash a string with SHA256
    /// </summary>
    /// <param name="inputText">The Text</param>
    /// <returns>The Hash from the Text</returns>
    private static string HashString(string inputText)
    {
        return Convert.ToHexString(SHA256.HashData(Encoding.Default.GetBytes(inputText)));
    }
}