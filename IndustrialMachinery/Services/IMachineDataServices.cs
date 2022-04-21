using IndustrialMachinery.Core;

namespace IndustrialMachinery.Services
{
    public interface IMachineDataServices
    {
        Task<List<Machine>> GetAllMachines();
        Task<Machine> GetMachineDetails(Guid Id);
        Task<Machine> AddMachine(Machine machine);
        Task UpdateMachine(Machine machine);
        Task DeleteMachine(Guid Id);

        Task<Machine> AddUpdatedMachine(Machine machine);

        Task<List<Machine>> GetTemperatures(Guid Id);
    }
}
