using Bet4ABestWorldPoC.Services.Request;
using System.Threading.Tasks;

namespace Bet4ABestWorldPoC.Services.Interfaces
{
    public interface IRegisterService
    {
        Task RegisterUser(RegisterRequest request);
    }
}
