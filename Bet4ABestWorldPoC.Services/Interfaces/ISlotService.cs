using Bet4ABestWorldPoC.Repositories.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface ISlotService
    {
        Task<List<Slot>> GetAllAsync();
        Task<List<Slot>> GetAllSlotThatContainsNameAsync(string name);
        Task<Slot> GetSlotByIdAsync(int id);
    }
}
