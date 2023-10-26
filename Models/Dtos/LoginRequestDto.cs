using System.ComponentModel.DataAnnotations;

namespace MediPortal_AuthService.Models.Dtos
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
