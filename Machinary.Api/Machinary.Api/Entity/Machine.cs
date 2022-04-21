using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Machinary.Api
{
    public class Machine
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public int Temperature { get; set; }
       

    }
}