using IndustrialMachinery.Core;

namespace IndustrialMachinery.Api.Repositories
{
    public class MachineRepository : IMachineRepository
    {

        public Task<Machine> AddMachine(Machine machine)
        {
            throw new NotImplementedException();
        }

        public void DeleteMachine(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Machine>> GetAllMachines()
        {
            throw new NotImplementedException();
        }

        public Task<Machine> GetMachineById(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<Machine> UpdateMachine(Machine machine)
        {
            throw new NotImplementedException();
        }
    }
}
