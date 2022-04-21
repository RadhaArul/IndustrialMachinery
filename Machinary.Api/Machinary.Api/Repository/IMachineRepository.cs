namespace Machinary.Api.Repository
{
    public interface IMachineRepository
    {
        IEnumerable<Machine> GetAllMachines();
        Machine GetMachineById(Guid Id);
        Machine AddMachine(Machine machine);
        void UpdateMachine(Machine machine);
        void DeleteMachine(Guid Id);
    }
}
