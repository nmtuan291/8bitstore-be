using _8bitstore_be.DTO;
using _8bitstore_be.Interfaces;
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

        public UserController(IRegistrationService registrationService, IConfiguration config)
        {
            _registrationService = registrationService;
            _config = config;
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

            RegistrationResponseDto response = await _registrationService.SignupAsync(user);

            if (!response.isSuccess)
            {
                return BadRequest(response.Errors);
            }

            return Ok("Sign up successfully");
        }

        [EnableCors("AllowFrontend")]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("CORS is working!");
        }
    }
}
