using Microsoft.AspNetCore.Identity;
namespace TP6.Models;
public class ApplicationUser : IdentityUser
{
    public string City { get; set; }
}
