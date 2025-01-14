using Microsoft.AspNetCore.Mvc;
using TP6.Services.ServiceContracts;
using TP6.Services;
using Microsoft.AspNetCore.Authorization;
[Route("api/User")]  // Changed from api/[controller]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("users")]
    public IActionResult GetUsers()
    {
        var users = _userService.GetUsersList();
        return Ok(users);
    }
}
