using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Services.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepositController : ControllerBase
    {
        private readonly IDepositService _depositService;

        public DepositController(IDepositService depositService)
        {
            _depositService = depositService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDeposits()
        {
            var result = await _depositService.GetDepositsForCurrentUserAsync();
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Bet([FromBody] DepositRequest request)
        {
            await _depositService.DepositAsync(request);
            return Ok();
        }
    }
}
