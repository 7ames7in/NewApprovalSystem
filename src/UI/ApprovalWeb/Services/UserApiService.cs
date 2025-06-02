using ApprovalWeb.Models;
using ApprovalWeb.Interfaces;
using System.Collections;
using System.Linq;
using UserService.Domain.Entities;
using System.Net.Http.Json; 

namespace ApprovalWeb.Services;

public class UserApiService<T> : IUserService<T> where T : User, new()
{
    private readonly HttpClient _http;

    public UserApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("UserApi");
    }

    // public async Task<bool> ValidateLoginAsync(string emailid)
    // {
    //     var response = await UserLoginAndInformationAsync(emailid);
    //     if (!response.IsSuccessStatusCode)
    //     {
    //         throw new InvalidOperationException($"Login validation failed for email ID {emailid}.");
    //     }
    //     return await response.Content.ReadFromJsonAsync<T>() ?? new T();
    // }

    public async Task<T> UserLoginAndInformationAsync(string emailid)
    {
        var response = await _http.PostAsJsonAsync("api/userservice/login", new { EmailId = emailid, Password = "1234" });
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Login validation failed for email ID {emailid}.");
        }
        return await response.Content.ReadFromJsonAsync<T>() ?? new T();
    }

    public async Task<T?> GetUserByUsernameAsync(string username)
    {
        return await _http.GetFromJsonAsync<T>($"api/users/username/{Uri.EscapeDataString(username)}");
    }

    public async Task<IEnumerable<T>> GetUsersByRoleAsync(string role)
    {
        return await _http.GetFromJsonAsync<IEnumerable<T>>($"api/users/role/{Uri.EscapeDataString(role)}") ?? new List<T>();
    }
    public async Task<IEnumerable<T>> GetAllUsersAsync()
    {
        return await _http.GetFromJsonAsync<IEnumerable<T>>("api/users") ?? new List<T>();
    }

    public async Task<T> GetUserByIdAsync(int id)
    {
        var user = await _http.GetFromJsonAsync<T>($"api/users/{id}");
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {id} not found.");
        }
        return user;
    }

    public async Task<IEnumerable<T>> SearchUsersAsync(string query)
    {
        return await _http.GetFromJsonAsync<IEnumerable<T>>($"api/search?query={Uri.EscapeDataString(query)}") ?? new List<T>();
    }

    public async Task<T> CreateUserAsync(T user)
    {
        var response = await _http.PostAsJsonAsync("api/users", user);
        return await response.Content.ReadFromJsonAsync<T>() ?? new T();
    }

    public async Task<bool> UpdateUserAsync(int id, T updatedUser)
    {
        var response = await _http.PutAsJsonAsync($"api/users/{id}", updatedUser);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/users/{id}");
        return response.IsSuccessStatusCode;
    }
}
