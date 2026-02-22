using AuthService.API.Models;
using AuthService.Application.Interfaces;
using AuthService.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    private readonly IJwtProvider _jwtProvider;

    public AuthController(IAuthService service, IJwtProvider jwtProvider)
    {
        _service = service;
        _jwtProvider = jwtProvider;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Email and password are required.");

        await _service.RegisterAsync(request.Email, request.Password);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Email and password are required.");

        var token = await _service.LoginAsync(request.Email, request.Password);
        return Ok(new { token });
    }
    

    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        return Challenge(new AuthenticationProperties 
        { 
            RedirectUri = "/api/auth/google-response" 
        }, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!result.Succeeded)
            return BadRequest("Google authentication failed");

        var email = result.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        if (email == null)
            return BadRequest("Email not found in Google response");

        var token = _jwtProvider.GenerateToken(email);
        var tokenJson = System.Text.Json.JsonSerializer.Serialize(token);
        var html = $"<html><body><script>if(window.opener){{window.opener.postMessage({{token: {tokenJson}}}, '*');}}\nwindow.close();</script></body></html>";
        return Content(html, "text/html");
    }


}
