using UsersManagement.Model;

namespace UsersManagement
{
    public interface IUserService
    {
        Task<ApiResponse> AddUserAsync(string name, string email, string password);
        Task<ApiResponse> GetUserAsync(string token);
        Task<ApiResponse> GetUserTokenAsync(string email, string password);
        Task<ApiResponse> RecoverPasswordAsync(string email);
    }
}