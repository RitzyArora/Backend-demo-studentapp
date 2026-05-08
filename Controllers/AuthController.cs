using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentCrudAppWithEFCoreCodeFirst.Dto;
using StudentCrudAppWithEFCoreCodeFirst.Models;
using StudentCrudAppWithEFCoreCodeFirst.Services;
using System.Security.Claims;

namespace StudentCrudAppWithEFCoreCodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task <IActionResult> Register(RegisterDto dto)
        {
            var user=new User
            {
                Username = dto.Username,
                Role = dto.Role,
                PasswordHash = dto.Password
            };
            await _authService.RegisterAsync(user);
            return Ok(new
            {
                message = "User Registered Successfully"
            });
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginDto loginUser)
        {
            var token = await _authService.LoginAsync(loginUser.Username, loginUser.Password);
            if (token == null)
                return Unauthorized("Invalid credentials !!");
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite=SameSiteMode.None,
                Expires=DateTime.UtcNow.AddHours(1)
            });
            //return Ok(new { Token = token });
            return Ok(new
            {
                message="Login Success"
            });
        }
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new { 
            
            username=User.Identity?.Name,
            role=User.Claims.First(c=>c.Type==ClaimTypes.Role).Value
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout() 
        {
            Response.Cookies.Delete("jwt");
            return Ok();
        }
    }
}
