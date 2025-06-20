using ApprovalService.Domain.Entities;

namespace ApprovalService.Application.Interfaces
{
     public interface IUserServiceProxy
     {
          Task<UserInfoDto?> GetUserByEmailAsync(string email);
          Task<UserInfoDto?> GetUserByIdAsync(string? employeeNumber);
     }
}