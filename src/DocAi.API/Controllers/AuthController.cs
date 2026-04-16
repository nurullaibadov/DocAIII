using DocAi.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DocAi.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        try { return Ok(await _authService.RegisterAsync(model.FullName, model.Email, model.Password)); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        try { return Ok(new { token = await _authService.LoginAsync(model.Email, model.Password) }); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
    {
        try { await _authService.ForgotPasswordAsync(model.Email); return Ok("Password reset link sent."); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }
}

public record RegisterDto(string FullName, string Email, string Password);
public record LoginDto(string Email, string Password);
public record ForgotPasswordDto(string Email);
