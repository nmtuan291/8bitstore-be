using System.Security.Claims;
using _8bitstore_be.DTO.User;
using _8bitstore_be.DTO;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace _8bitstore_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly ILoginService _loginService;
        private readonly IUserService _userService;

        public UserController(IRegistrationService registrationService, ILoginService loginService, IUserService userService)
        {
            _registrationService = registrationService;
            _loginService = loginService;
            _userService = userService;

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
            Console.WriteLine("asadasdasd");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (user == null)
            {
                return BadRequest("User data is required");
            }

            AuthResponseDto response = await _loginService.LoginAsync(user);

            if (response.User == null)
            {
                return StatusCode(500, "An error occurred during login.");
            }


            if (!response.isSuccess)
            {
                return BadRequest(response.Errors);
            }


            return Ok("Login successful");
        }

        [Authorize]
        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
            Console.WriteLine(userId);

            if (!ModelState.IsValid || userId == null)
            {
                return BadRequest("Missing username");
            }

            var user = await _loginService.GetUserAsync(userId);

            if (user == null)
            {
                Console.WriteLine("dfsfsdfsdfdsff");
                return NotFound("Cannot find the user");
            }

            return Ok(user);
        }

        [HttpPatch("update-address")]
        public async Task<IActionResult> UpdateAddress([FromBody] UserDto request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
             
            if (string.IsNullOrEmpty(user))
            {
                return Unauthorized();
            }

            try
            {
                await _userService.ChangeAddressAsync(user, request.Address, request.City, request.District, request.SubDistrict);
                return Ok("Change address successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _loginService.LogoutAsync();
                return Ok(new StatusResponse<string>
                {
                    Status = "SUCCESS",
                    Message = "Logout successfully"
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
