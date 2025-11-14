using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.UnitsOfWork.Interfaces
{
    public interface IStatesUnifOfWork
    {
        Task<ActionResponse<State>>GetAsync(int Id);    
        Task<ActionResponse<IEnumerable<State>>> GetAsync();
    }
}
