using Bet4ABestWorldPoC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogoutController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public LogoutController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = _tokenService.GetCurrentUserId();
            await _tokenService.InvalidateTokenAsync(userId);
            return Ok();
        }
    }
}
