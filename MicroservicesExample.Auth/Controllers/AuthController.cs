using MicroservicesExample.Auth.Models;
using MicroservicesExample.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesExample.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService userService)
        {
            _authService = userService;
        }

        [HttpPost]
        [Route("sign-in")]
        public async Task<IActionResult> SignIn(User model)
        {
            var res = await _authService.SignIn(model);
            return Ok(res);
        }
    }
}
