using Machinary.Api.Data;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace Machinary.Api.Repository
{
    public class MachineRepository : IMachineRepository
    {
        private readonly MachinaryApiContext db;
        public MachineRepository(MachinaryApiContext db)
        {
            this.db = db;
        }

        public Machine AddMachine(Machine machine)
        {
            var addedEntity = db.Machine.Add(machine);
            db.SaveChanges();
            return addedEntity.Entity;
        }

        public void DeleteMachine(Guid Id)
        {
            var foundMachine = db.Machine.FirstOrDefault(m=>m.Id == Id);
            if (foundMachine == null) return;

            db.Machine.Remove(foundMachine);
            db.SaveChanges();
        }

        public IEnumerable<Machine> GetAllMachines()
        {
            return db.Machine;
        }

        public Machine GetMachineById(Guid Id)
        {
            return db.Machine.FirstOrDefault(m=>m.Id == Id);
        }

        public void UpdateMachine(Machine machine)
        {
            
                db.Entry(machine).State = EntityState.Modified;
                db.SaveChanges();

           

        }
    }
}
