using System.ComponentModel.DataAnnotations;

namespace BestWesternWeb.Models
{
    public class Credential
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}