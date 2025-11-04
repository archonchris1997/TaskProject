
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TaskProject.Models;
using TaskProject.Models.Models.DTO;

namespace TaskProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController (
 UserManager<ApplicationUser> _userManager,
 SignInManager<ApplicationUser> _signInManager,
RoleManager<IdentityRole> _roleManager,
IConfiguration _config
    ) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = new ApplicationUser
        {
          UserName = dto.Email,
          Email = dto.Email,
        };
        
        //Criar 
        var result =  await _userManager.CreateAsync(user, dto.Password );

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        // Depois de Criar vou *associar a um role*
        await _userManager.AddToRoleAsync(user, "User");
        return Ok("Utilizador registado com sucesso!");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null) return Unauthorized("Utilizador não encontrado.");

        var valid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!valid) return Unauthorized("Password incorreta.");

        // 🔐 Gerar token JWT
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? "")
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var jwt = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { accessToken = tokenString });
    }
        
}
public record RegisterDto(string Email, string Password);
public record LoginDto(string Email, string Password);