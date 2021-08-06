using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Repositories.Interfaces;
using Bet4ABestWorldPoC.Services.Exceptions;
using Bet4ABestWorldPoC.Services.Interfaces;
using System.Collections.Generic;
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

        public async Task<List<Slot>> GetAllAsync()
        {
            return await _slotRepository.GetAllAsync();
        }

        public async Task<List<Slot>> GetAllSlotThatContainsNameAsync(string name)
        {
            return await _slotRepository.GetAllWhereAsync(w => w.Name.Contains(name));
        }

        public async Task<Slot> GetSlotByIdAsync(int id)
        {
            var slot = await _slotRepository.GetByIdAsync(id);
            if (slot == null)
            {
                throw new SlotNotFoundException();
            }
            return slot;
        }
    }
}
