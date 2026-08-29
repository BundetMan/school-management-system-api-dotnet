using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Models.People;
using SchoolAPI.Services;
using SchoolAPI.DTOs;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return Unauthorized("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid credentials");

            var token = await _tokenService.CreateTokenAsync(user);
            return Ok(new { Token = token });
        }

        #region No need register for user because we have user controller can use by admin
        /*
        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto dto)
        {

            if(await _userManager.FindByEmailAsync(dto.Email) != null)
            {
                return Conflict(new {Message = $"Email '{dto.Email}' is already in use."});
            }
            if(await _userManager.FindByNameAsync(dto.Username) != null)
            {
                return Conflict(new {Message = $"Username '{dto.Username}' is already in use."});
            }

            var user = new User
            {
                UserName = dto.Username,
                Email = dto.Email,
                Id = Guid.NewGuid().ToString(),
                Status = "Active"
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }

            await _userManager.AddToRoleAsync(user, dto.Role ?? "Student"); // Default to "Student" role if not specified

            var token = await _tokenService.CreateTokenAsync(user);
            return Ok("User registered successfully");
        }
        */
        #endregion
    }
}