using _8bitstore_be.DTO;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace _8bitstore_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly IConfiguration _config;
        private readonly ILoginService _loginService;

        public UserController(IRegistrationService registrationService, ILoginService loginService)
        {
            _registrationService = registrationService;
            _loginService = loginService;

        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserForRegistrationDto user)
        {
            if (user == null)
            {
                return BadRequest("User data is required");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuthResponseDto response = await _registrationService.SignupAsync(user);

            if (!response.isSuccess)
            {
                return BadRequest(response.Errors);
            }

            return Ok("Sign up successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (user == null)
            {
                return BadRequest("User data is required");
            }

            AuthResponseDto response = await _loginService.LoginAsync(user);

            if (response == null)
            {
                return StatusCode(500, "An error occurred during login.");
            }


            if (!response.isSuccess)
            {
                return BadRequest(response.Errors);
            }

            // Set cookies with the access token and refresh token
            var accessToken = response.User.AccessToken;
            var refreshToken = response.User.RefreshToken;

            Response.Cookies.Append("AccessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok("Login successful");
        }
    }
}
