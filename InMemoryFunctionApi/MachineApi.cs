using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IndustrialMachinery.Core;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
#nullable disable

namespace InMemoryFunctionApi
{
    public static class MachineApi
    {
        static List<Machine> items = new List<Machine>();
        
        


        [FunctionName("CreateMachine")]
        [Obsolete]
        public static async Task<IActionResult> CreateMachine(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "machines")] HttpRequest req, TraceWriter log)
        {
            log.Info("Creating a new Machine list item");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<MachineCreateModel>(requestBody);

            var machine = new Machine() { Name = input.Name,Temperature=input.Temperature };
            items.Add(machine);
            return new OkObjectResult(machine);
        }

        [FunctionName("Get")]
        [Obsolete]
        public static IActionResult Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "machines")] HttpRequest req, TraceWriter log)
        {
            log.Info("Getting Machine list items");
            return new OkObjectResult(items.AsEnumerable());
        }

        [FunctionName("GetMachineById")]
        [Obsolete]
        public static IActionResult GetMachineById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "machines/{id}")] HttpRequest req,
            TraceWriter log, Guid id)
        {
            var machine = items.FirstOrDefault(t => t.Id == id);
            if (machine == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(machine);
        }

        [FunctionName("UpdateMachine")]
        [Obsolete]
        public static async Task<IActionResult> UpdateMachine(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "machines/{id}")] HttpRequest req,
            TraceWriter log, Guid id)
        {
            var machine = items.FirstOrDefault(t => t.Id == id);
            if (machine == null)
            {
                return new NotFoundResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<MachineUpdateModel>(requestBody);

            machine.Status = updated.Status;
           // if (!string.IsNullOrEmpty(updated.Name))
            //{
                machine.Name = updated.Name;
                machine.Temperature = updated.Temperature;
            //}

            return new OkObjectResult(machine);
        }

        [FunctionName("DeleteMachine")]
        [Obsolete]
        public static IActionResult DeleteMachine(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "machines/{id}")] HttpRequest req,
            TraceWriter log, Guid id)
        {
            var machine = items.FirstOrDefault(t => t.Id == id);
            if (machine == null)
            {
                return new NotFoundResult();
            }
            items.Remove(machine);
            
            return new OkResult();
        }
    }
}
