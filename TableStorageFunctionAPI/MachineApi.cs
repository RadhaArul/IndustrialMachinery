using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using System.Linq;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Cosmos.Table.Queryable;
using Microsoft.Azure.Storage.Blob;
using IndustrialMachinery.Core;

namespace TableStorageFunctionAPI
{
    public static class MachineApi
    {

        [FunctionName("CreateMachine")]
        public static async Task<IActionResult> CreateMachine(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "machines")] HttpRequest req,
            [Table("machines", Connection = "AzureWebJobsStorage")] IAsyncCollector<MachineTableEntity> machineTable,
            
            ILogger log)
        {
            log.LogInformation("Creating a new machine");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<MachineCreateModel>(requestBody);

            var machine = new Machine() { Name = input.Name, Temperature = input.Temperature };
            await machineTable.AddAsync(machine.ToMachineTableEntity());
            return new OkObjectResult(machine);
        }

        [FunctionName("GetMachines")]
        public static async Task<IActionResult> GetMachines(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "machines")] HttpRequest req,
            [Table("machines", Connection = "AzureWebJobsStorage")] CloudTable table,
            ILogger log)
        {
            log.LogInformation("Getting machines");
            var query = new TableQuery<MachineTableEntity>();
            var segment = await table.ExecuteQuerySegmentedAsync(query, null);
            var result = segment.Select(Mappings.ToMachine).ToList();
            return new OkObjectResult(result);
        }

        [FunctionName("GetMachineById")]
        public static IActionResult GetMachineById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "machines/{id}")] HttpRequest req,
            [Table("machines", "Machine", "{id}", Connection = "AzureWebJobsStorage")] MachineTableEntity machine,
            ILogger log, Guid Id)
        {
            log.LogInformation("Getting machine by id");
            // var machineobj = machine.FirstOrDefault(t => t.Id == id);
            if (machine == null)
            {
                log.LogInformation($"Machine {Id} not found");
                return new NotFoundResult();
            }
            //if(temperature == null)
            return new OkObjectResult(machine.ToMachine());

            
        }

        [FunctionName("GetTemperature")]
        public static async Task<IActionResult> GetTemperature(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "updatedmachines/{id}")] HttpRequest req,
            [Table("updatedmachines", Connection = "AzureWebJobsStorage")] CloudTable temp,
            ILogger log, Guid Id)
        {
            log.LogInformation("Getting Temperature of the machine");
            var query = new TableQuery<MachineUpdatedTableEntity>();
            var segment = await temp.ExecuteQuerySegmentedAsync(query, null);
            var result = segment.Select(Mappings.ToUpdatedMachine).ToList();
            var selectedresult=result.Where(x => x.Id==Id);
            return new OkObjectResult(selectedresult);


        }

        [FunctionName("UpdateMachine")]
        public static async Task<IActionResult> UpdateMachine(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "machines/{id}")] HttpRequest req,
            [Table("machines", Connection = "AzureWebJobsStorage")] CloudTable machineTable,
            ILogger log, Guid Id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<MachineUpdateModel>(requestBody);
            var findOperation = TableOperation.Retrieve<MachineTableEntity>("Machine", Id.ToString());
            var findResult = await machineTable.ExecuteAsync(findOperation);
            if (findResult.Result == null)
            {
                return new NotFoundResult();
            }
            var existingRow = (MachineTableEntity)findResult.Result;
            existingRow.Name = updated.Name;
            existingRow.Status = updated.Status;
            existingRow.Temperature = updated.Temperature;


            var replaceOperation = TableOperation.Replace(existingRow);
            await machineTable.ExecuteAsync(replaceOperation);
            return new OkObjectResult(existingRow.ToMachine());
        }
        //Method 1 for Delete

        //[FunctionName("DeleteMachine")]
        //public static async Task<IActionResult> DeleteMachine(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "machines/{id}")] HttpRequest req,
        //    [Table("machines", Connection = "AzureWebJobsStorage")] CloudTable machineTable,
        //    ILogger log, Guid Id)
        //{
        //    var deleteOperation = TableOperation.Delete(new TableEntity()
        //    { PartitionKey = "Machine", RowKey = Id.ToString(), ETag = "*" });

        //    try
        //    {
        //        var deleteResult = await machineTable.ExecuteAsync(deleteOperation);
        //    }
        //    catch (StorageException e) when (e.RequestInformation.HttpStatusCode == 404)
        //    {
        //        return new NotFoundResult();
        //    }
        //    return new OkResult();
        //}

        //Method 2 for Delete and move to queue
        [FunctionName("DeleteMachine")]
        public static async Task<IActionResult> DeleteMachine(
         [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "machines/{id}")] HttpRequest req,
         [Table("machines", "Machine", "{id}", Connection = "AzureWebJobsStorage")] MachineTableEntity machineTableToDelete,
          [Table("machines", Connection = "AzureWebJobsStorage")] CloudTable machineTable,
          [Queue("machines", Connection = "AzureWebJobsStorage")] IAsyncCollector<Machine> machineQueue,
         Guid Id,
         ILogger log)
        {
            log.LogInformation("Delete Machine");

            if (machineTableToDelete is null) return new BadRequestResult();

            log.LogInformation("moved to queue");
            await machineQueue.AddAsync(machineTableToDelete.ToMachine());
            
            var operation = TableOperation.Delete(machineTableToDelete);
            var res = await machineTable.ExecuteAsync(operation);

            return new NoContentResult();
        }


        [FunctionName("CreateUpdatedMachine")]
        public static async Task<IActionResult> CreateUpdatedMachine(
          [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "updatedmachines")] HttpRequest req,
          [Table("updatedmachines", Connection = "AzureWebJobsStorage")]  CloudTable machineupdatedTable,
          [Table("machines", Connection = "AzureWebJobsStorage")] CloudTable machineTable,
          ILogger log)
        {
            log.LogInformation("Creating a new updated machine");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<MachineUpdatedTableEntityModel>(requestBody);

            var machine = new Machine() { 
                Id=input.Id,
                Temperature = input.Temperature };

            var m = machine.ToMachineUpdatedTableEntity();

            await machineupdatedTable.CreateIfNotExistsAsync();

            await machineupdatedTable.ExecuteAsync(TableOperation.Insert(m));
            

            var findOperation = TableOperation.Retrieve<MachineTableEntity>("Machine", input.Id.ToString());
            var findResult = await machineTable.ExecuteAsync(findOperation);
            if (findResult.Result == null)
            {
                return new NotFoundResult();
            }
            var existingRow = (MachineTableEntity)findResult.Result;
            existingRow.Date=m.Date;
            existingRow.Temperature = m.Temperature;

            log.LogInformation("Temperature ------ { existingRow.Temperature}");

            var replaceOperation = TableOperation.Replace(existingRow);
            await machineTable.ExecuteAsync(replaceOperation);

            return new OkObjectResult(machine);
        }


        [FunctionName("GetRemovedFromQueue")]
        public static async Task GetRemovedFromQueue(
          [QueueTrigger("machines", Connection = "AzureWebJobsStorage")] Machine machine,
          [Blob("done", Connection = "AzureWebJobsStorage")] CloudBlobContainer blobContainer,
          ILogger log)
        {
            log.LogInformation("Queue trigger started...");

            await blobContainer.CreateIfNotExistsAsync();
            var blob = blobContainer.GetBlockBlobReference($"{machine.Id}.txt");
            await blob.UploadTextAsync($"{machine.Id} is completed");
        }

    }
}
