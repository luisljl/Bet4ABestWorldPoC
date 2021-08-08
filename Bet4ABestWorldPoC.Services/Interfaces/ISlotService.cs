using Bet4ABestWorldPoC.Repositories.Entities;
using Bet4ABestWorldPoC.Services.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface ISlotService
    {
        Task<List<SlotResponse>> GetAllAsync();
        Task<List<SlotResponse>> GetAllSlotThatContainsNameAsync(string name);
        Task<Slot> GetSlotByIdAsync(int slotId);
        Task<string> GetSlotNameById(int slotId);
    }
}
