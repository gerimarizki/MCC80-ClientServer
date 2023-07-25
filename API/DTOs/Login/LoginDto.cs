

using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Login
{
    public class LoginDto
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
