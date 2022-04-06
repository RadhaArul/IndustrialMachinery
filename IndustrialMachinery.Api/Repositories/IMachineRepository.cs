using IndustrialMachinery.Core;

namespace IndustrialMachinery.Api.Repositories
{
    public interface IMachineRepository
    {
        Task<IEnumerable<Machine>> GetAllMachines();
        Task<Machine> GetMachineById(Guid Id);
        Task<Machine> AddMachine(Machine machine);
        Task<Machine> UpdateMachine(Machine machine);
        void DeleteMachine(Guid Id);
    }
}
