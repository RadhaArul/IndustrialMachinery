#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Machinary.Api;
using Machinary.Api.Data;
using Machinary.Api.Repository;

namespace Machinary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachinesController : ControllerBase
    {
        private readonly IMachineRepository repo;

        public MachinesController(IMachineRepository repo)
        {
            this.repo = repo;
        }

        // GET: api/Machines
        [HttpGet]
        public  ActionResult<IEnumerable<Machine>> GetMachine()
        {
            return Ok( repo.GetAllMachines());
        }

        // GET: api/Machines/5
        [HttpGet("{id}")]
        public ActionResult<Machine> GetMachine(Guid id)
        {
            var machine =  repo.GetMachineById(id);

            if (machine == null)
            {
                return NotFound();
            }

            return machine;
        }

        // PUT: api/Machines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public  IActionResult UpdateMachine([FromBody] Machine machine)
        {
            if (machine == null)
                return BadRequest();

            if (machine.Name == string.Empty )
            {
                ModelState.AddModelError("Name", "The name shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            repo.UpdateMachine(machine);

            return NoContent();

        }

        // POST: api/Machines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Machine> CreateMachine([FromBody]Machine machine)
        {
            if (machine == null)
                return BadRequest();

            if (machine.Name == string.Empty)
            {
                ModelState.AddModelError("Name", "The name shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdMachine = repo.AddMachine(machine);

            return Created("machine", createdMachine);
        }

        // DELETE: api/Machines/5
        [HttpDelete("{id}")]
        public IActionResult DeleteMachine(Guid id)
        {
           

            var machineToDelete = repo.GetMachineById(id);
            if (machineToDelete == null)
                return NotFound();

               repo.DeleteMachine(id);

            return NoContent();//success
        }

        
    }
}
