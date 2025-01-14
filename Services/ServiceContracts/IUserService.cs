using TP6.Models;
namespace TP6.Services.ServiceContracts;
using TP6.Models;
public interface IUserService
{
    IEnumerable<ApplicationUser> GetUsersList();
}
