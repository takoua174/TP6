using System.ComponentModel.DataAnnotations;
namespace TP6.Models
{
    public class RegisterModel
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
}