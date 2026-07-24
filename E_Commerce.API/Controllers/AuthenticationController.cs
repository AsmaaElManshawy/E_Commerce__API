using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.IdentityDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        // login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
            => ToActionResult(await _authService.LoginAsync(loginDto));
        // register
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto , CancellationToken ct = default)
            => ToActionResult(await _authService.RegisterAsync(registerDto , ct));



    }
}
