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

        public async Task<List<Slot>> GetAll()
        {
            return await _slotRepository.GetAll();
        }

        public async Task<List<Slot>> GetAllSlotThatContainsName(string name)
        {
            return await _slotRepository.GetAllWhere(w => w.Name.Contains(name));
        }

        public async Task<Slot> GetSlotById(int id)
        {
            var slot = await _slotRepository.GetById(id);
            if (slot == null)
            {
                throw new SlotNotFoundException();
            }
            return slot;
        }
    }
}
