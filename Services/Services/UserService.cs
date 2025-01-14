using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TP6.Models;
using TP6.Services.ServiceContracts;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public IEnumerable<ApplicationUser> GetUsersList()
    {
        return _userManager.Users; // IQueryable<ApplicationUser>
    }
}
