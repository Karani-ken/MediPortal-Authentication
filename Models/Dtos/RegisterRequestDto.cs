using System.ComponentModel.DataAnnotations;

namespace MediPortal_AuthService.Models.Dtos
{
    public class RegisterRequestDto
    {
     
        public string firstname { get; set; } = string.Empty;
    
        public string lastname { get; set; } = string.Empty;
       
        public string? surname { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;

       
        public string? Password { get; set; } = string.Empty;
         public string? speciality { get; set; } = string.Empty;
        public string? HospitalName { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;
         public string? LicenseUrl { get; set; } = string.Empty;
       // public IFormFile? License { get; set; }

    }
}
