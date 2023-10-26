using MediPortal_AuthService.Models;

namespace MediPortal_AuthService.Services.IService
{
    public interface IJwtInterface
    {
        string GenerateToken(User user, IEnumerable<string> roles);
    }
}
