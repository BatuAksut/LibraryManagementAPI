using API.Models.Dtos;
using API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }



        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var identityUser = new IdentityUser()
            {
                UserName = registerDto.Username,
                Email = registerDto.Username
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerDto.Password);
            if (identityResult.Succeeded)
            {
                //Add roles to the user if provided
                if (registerDto.Roles != null && registerDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User registered successfully");
                    }
                }

            }

            return BadRequest("User registration failed");

        }



        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var identityUser = await userManager.FindByEmailAsync(loginDto.Username);
            if (identityUser != null && await userManager.CheckPasswordAsync(identityUser, loginDto.Password))
            {

                var roles = await userManager.GetRolesAsync(identityUser);
                if (roles != null && roles.Count > 0)
                {
                    var jwtToken = tokenRepository.CreateJWTToken(identityUser, roles.ToList());
                    var response = new LoginResponseDto()
                    {
                        JwtToken = jwtToken
                    };
                    return Ok(response);
                }



            }
            return BadRequest("Invalid username or password");
        }
    }
}
