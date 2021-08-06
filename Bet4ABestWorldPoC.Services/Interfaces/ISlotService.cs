using Bet4ABestWorldPoC.Repositories.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface ISlotService
    {
        Task<List<Slot>> GetAll();
        Task<List<Slot>> GetAllSlotThatContainsName(string name);
        Task<Slot> GetSlotById(int id);
    }
}
