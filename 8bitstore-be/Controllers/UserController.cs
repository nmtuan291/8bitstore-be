using System.Security.Claims;
using _8bitstore_be.DTO.User;
using _8bitstore_be.DTO;
using _8bitstore_be.Interfaces;
using _8bitstore_be.Interfaces.Services;
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
            AuthResponseDto response = await _registrationService.SignupAsync(user);

            if (!response.isSuccess)
            {
                return BadRequest(response.Errors);
            }

            return Ok();
        }
 
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            AuthResponseDto response = await _loginService.LoginAsync(user);

            if (response.User == null)
            {
                return Unauthorized();
            }


            if (!response.isSuccess)
            {
                return BadRequest(response.Errors);
            }


            return Ok();
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
        
        [Authorize]
        [HttpPut("address/update")]
        public async Task<IActionResult> UpdateAddress([FromBody] AddressDto address)
        {
            try
            {
                await _userService.ChangeAddressAsync(address);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPost("address/add")]
        public async Task<IActionResult> AddAddress([FromBody] AddressDto address)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            
            await _userService.AddAddressAsync(address, userId);
            return Ok();
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<IActionResult> GetAddress()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var addresses = await _userService.GetAddressesByUserIdAsync(userId);
            return Ok(addresses);
        }

        [Authorize]
        [HttpDelete("address/delete/{addressId}")]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            await _userService.DeleteAddressAsync(addressId);
            return Ok();
        }
        
        [Authorize]
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
