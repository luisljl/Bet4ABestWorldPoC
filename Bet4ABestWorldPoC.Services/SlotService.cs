using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using Bet4ABestWorldPoC.Services.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services
{
    public class SlotService : ISlotService
    {
        private readonly ISlotRepository _slotRepository;
        
        public SlotService(ISlotRepository slotRepository)
        {
            _slotRepository = slotRepository;
        }

        public async Task<List<SlotResponse>> GetAllAsync()
        {
            var slots = await _slotRepository.GetAllAsync();
            return MapListSlotsToListSlotResponse(slots);
        }

        public async Task<List<SlotResponse>> GetAllSlotThatContainsNameAsync(string name)
        {
            var slots = await _slotRepository.GetAllWhereAsync(w => w.Name.Contains(name));
            return MapListSlotsToListSlotResponse(slots);
        }

        public async Task<Slot> GetSlotByIdAsync(int slotId)
        {
            var slot = await _slotRepository.GetByIdAsync(slotId);
            if (slot == null)
            {
                throw new SlotNotFoundException();
            }
            return slot;
        }

        public async Task<string> GetSlotNameById(int slotId)
        {
            var slot = await GetSlotByIdAsync(slotId);
            return slot.Name;
        }

        private List<SlotResponse> MapListSlotsToListSlotResponse(IEnumerable<Slot> slots)
        {
            slots ??= Enumerable.Empty<Slot>();

            return slots.Select(s => new SlotResponse()
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }
    }
}
