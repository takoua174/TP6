using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TP6.Models;
using Microsoft.AspNetCore.Authorization;
[Route("api/RoleManagement")]
[ApiController]
public class RoleManagementController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;
    readonly UserManager<ApplicationUser> _userManager;

    public RoleManagementController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet("roles")]
    public IActionResult GetRoles()
    {
        var roles = _roleManager.Roles;
        return Ok(roles);
    }

    [HttpPost("create-role")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (string.IsNullOrEmpty(roleName))
            return BadRequest("Role name cannot be empty.");

        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

        if (result.Succeeded)
            return Ok("Role created successfully.");
        return BadRequest(result.Errors);
    }
    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRoleToUser(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound("User not found.");

        if (!await _roleManager.RoleExistsAsync(roleName))
            return NotFound("Role not found.");

        var result = await _userManager.AddToRoleAsync(user, roleName);

        if (result.Succeeded)
            return Ok("Role assigned successfully.");
        return BadRequest(result.Errors);
    }

}
