using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Cosmos.Table;
#nullable disable
namespace IndustrialMachinery.Core
{
    public class Machine
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; }
        public DateTime Date { get; set; }= DateTime.UtcNow;
        public bool Status { get; set; }=false;
        public int Temperature { get; set; }

    }

    public class MachineCreateModel
    {
        public string Name { get; set; }
        public int Temperature { get; set; }
    }

    public class MachineUpdateModel
    {
        public string Name { get; set; }
        public bool Status { get; set; }

        public int Temperature { get; set; }
    }

    public class MachineTableEntity : TableEntity
    {
        [Required]
        public string Name { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public bool Status { get; set; } = false;
        public int Temperature { get; set; }
    }
    public class MachineUpdatedTableEntity : TableEntity
    {

        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int Temperature { get; set; }
    }
    public class MachineUpdatedTableEntityModel : TableEntity
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; } 
        public int Temperature { get; set; }
    }
    public static class Mappings
    {
        public static MachineTableEntity ToMachineTableEntity(this Machine machine)
        {
            return new MachineTableEntity()
            {
                PartitionKey = "Machine",
                RowKey = machine.Id.ToString(),
                Name = machine.Name,
                Date = machine.Date,
                Status = machine.Status,
                Temperature = machine.Temperature
            };
        }
        public static MachineUpdatedTableEntity ToMachineUpdatedTableEntity(this Machine machine)
        {
            Random rand = new Random();
            return new MachineUpdatedTableEntity()
            {
                PartitionKey = machine.Id.ToString(),
                //RowKey = DateTime.UtcNow.ToString(),
                RowKey= machine.Id.ToString()+rand.Next(1,100),
                Id = machine.Id,
                Date = DateTime.Now,
                Temperature = rand.Next(-20, 45)
            };
        }
        public static Machine ToMachine(this MachineTableEntity machinetable)
        {
            return new Machine()
            {
                Id = Guid.Parse(machinetable.RowKey),
                Name = machinetable.Name,
                Date= machinetable.Date,
                Status= machinetable.Status,
                Temperature = machinetable.Temperature
            };
        }
        public static Machine ToUpdatedMachine(this MachineUpdatedTableEntity updatedmachinetable)
        {
            return new Machine()
            {
                Id=updatedmachinetable.Id,
                Date = updatedmachinetable.Date,
                Temperature = updatedmachinetable.Temperature

            };
        }

    }
}