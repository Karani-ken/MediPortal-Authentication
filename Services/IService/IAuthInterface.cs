using MediPortal_AuthService.Models;

using MediPortal_AuthService.Models.Dtos;

namespace MediPortal_AuthService.Services.IService
{
    public interface IAuthInterface
    {
        Task<string> RegisterUser(RegisterRequestDto newUser);
        Task<string> DeleteUser(Guid userId);
        Task<List<User>> GetUsers();
        Task<string> UpdateUser(User updatedUser);

        
        Task<LoginResponseDto> LoginUser (LoginRequestDto Loginrequest);

        Task<bool> AssignUserRole(string email, string Rolename);
    }
}
