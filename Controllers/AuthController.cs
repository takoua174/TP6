using TP6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TP6.JWTBearerConfig;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace TP6.Controllers;
public class AuthController : ControllerBase
{
    private readonly JWTBearerTokenSettings jwtBearerTokenSettings;
    private readonly UserManager<ApplicationUser> userManager;
    public AuthController(IOptions<JWTBearerTokenSettings> jwtTokenOptions, UserManager<ApplicationUser> userManager)
    {
        this.jwtBearerTokenSettings = jwtTokenOptions.Value;
        this.userManager = userManager;
    }
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel userDetails)
    {
        if (!ModelState.IsValid || userDetails == null)
        {
            return new BadRequestObjectResult(new
            {
                Message = "User Registration Failed"
            });
        }
        var applicationUser = new ApplicationUser()
        {
            UserName = userDetails.username,
            Email = userDetails.Email,
            City = ""
        };
        var result = await userManager.CreateAsync(applicationUser, userDetails.Password);
        if (!result.Succeeded)
        {
            var dictionary = new ModelStateDictionary();
            foreach (IdentityError error in result.Errors)
            {
                dictionary.AddModelError(error.Code, error.Description);
            }
            return new BadRequestObjectResult(new
            {
                Message = "User Registration Failed",
                Errors =
           dictionary
            });
        }
        return Ok(new { Message = "User Reigstration Successful" });
    }
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel credentials)
    {
        ApplicationUser applicationUser;
        if (!ModelState.IsValid || credentials == null || (applicationUser = await ValidateUser(credentials)) == null)
        {
            return new BadRequestObjectResult(new
            {
                Message = "Login failed"
            });
        }
        var token = GenerateToken(applicationUser);
        return Ok(new { Token = token, Message = "Success" });
    }
    [HttpPost]
    [Route("Logout")]
    public async Task<IActionResult> Logout()
    {
        // Well, What do you want to do here ?
        // Wait for token to get expired OR
        // Maintain token cache and invalidate the tokens after logout 
        return Ok(new { Token = "", Message = "Logged Out" });
    }
    private async Task<ApplicationUser> ValidateUser(LoginModel credentials)
    {
        var applicationUser = await userManager.FindByEmailAsync(credentials.Email);
        if (applicationUser != null)
        {
            var result = userManager.PasswordHasher.VerifyHashedPassword(applicationUser, applicationUser.PasswordHash, credentials.Password);
            return result == PasswordVerificationResult.Failed ? null : applicationUser;
        }
        return null;
    }
    private async Task<string> GenerateToken(ApplicationUser applicationUser)
    {
        if (applicationUser == null)
            throw new ArgumentNullException(nameof(applicationUser), "Application user cannot be null.");

        // Ensure jwtBearerTokenSettings is properly initialized
        if (jwtBearerTokenSettings == null)
            throw new InvalidOperationException("JWT Bearer Token settings must be initialized.");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

        // Validate ExpireTimeInSeconds
        if (jwtBearerTokenSettings.ExpireTimeInSeconds <= 0)
            throw new ArgumentException("Expire time must be greater than 0 seconds.", nameof(jwtBearerTokenSettings.ExpireTimeInSeconds));

        var now = DateTime.UtcNow;

        // Retrieve roles for the user
        var roles = await userManager.GetRolesAsync(applicationUser);

        // Create claims
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, applicationUser.Id), // User ID
        new Claim(ClaimTypes.Name, applicationUser.UserName ?? string.Empty), // Username
        new Claim(ClaimTypes.Email, applicationUser.Email ?? string.Empty), // Email
    };

        // Add role claims
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            NotBefore = now, // Token becomes valid immediately
            Expires = now.AddSeconds(jwtBearerTokenSettings.ExpireTimeInSeconds), // Expiry time
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = jwtBearerTokenSettings.Audience,
            Issuer = jwtBearerTokenSettings.Issuer
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}
