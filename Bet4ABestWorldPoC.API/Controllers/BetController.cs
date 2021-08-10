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
    public class BetController : ControllerBase
    {
        private readonly IBetService _betService;

        public BetController(IBetService betService)
        {
            _betService = betService;
        }


        [HttpGet("GetBetsBySlot/{slotId}")]
        [Authorize]
        public async Task<IActionResult> GetBetsBySlot(int slotId)
        {
            var result = await _betService.GetCurrentUserBetHistoricBySlotAsync(slotId);
            return Ok(new JsonResult(result));
        }

        [HttpGet("GetWinningBetsBySlot/{slotId}")]
        [Authorize]
        public async Task<IActionResult> GetWinningBets(int slotId)
        {
            var result = await _betService.GetCurrentUserWinningBetHistoricBySlotAsync(slotId);
            return Ok(new JsonResult(result));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBets()
        {
            var result = await _betService.GetCurrentUserBetHistoricAsync();
            return Ok(new JsonResult(result));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Bet([FromBody] BetRequest request)
        {
            var response = await _betService.Bet(request);
            return Ok(new JsonResult(response));
        }
    }
}
