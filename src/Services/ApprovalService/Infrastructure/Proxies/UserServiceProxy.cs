using ApprovalService.Application.Interfaces;
using ApprovalService.Domain.Entities;
using System.Net.Http;
using System.Net.Http.Json;

namespace ApprovalService.Infrastructure.Proxies
{
    public class UserServiceProxy : IUserServiceProxy
    {
        private readonly HttpClient _httpClient;

        public UserServiceProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserInfoDto?> GetUserByEmailAsync(string email)
        {
            return await _httpClient.GetFromJsonAsync<UserInfoDto>($"api/userservice/users/email/{email}");
        }

        public async Task<UserInfoDto?> GetUserByIdAsync(string? employeeNumber)
        {
            return await _httpClient.GetFromJsonAsync<UserInfoDto>($"api/userservice/users/{employeeNumber}");
        }
    }
}