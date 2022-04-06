using System.ComponentModel.DataAnnotations;

namespace IndustrialMachinery.Core
{
    public class Machine
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public string Data { get; set; }


    }
}