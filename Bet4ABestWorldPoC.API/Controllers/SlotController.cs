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
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [HttpGet("GetById/{slotId}")]
        [Authorize]
        public async Task<IActionResult> GetSlot(int slotId)
        {
            var response = await _slotService.GetSlotByIdAsync(slotId);
            return Ok(new JsonResult(response));
        }

        [HttpGet("GetByName/{slotName}")]
        [Authorize]
        public async Task<IActionResult> GetSlotsByName(string slotName)
        {
            var response = await _slotService.GetAllSlotThatContainsNameAsync(slotName);
            return Ok(new JsonResult(response));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllSlots()
        {
            var response = await _slotService.GetAllAsync();
            return Ok(new JsonResult(response));
        }
    }
}
