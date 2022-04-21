using IndustrialMachinery.Core;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
#nullable disable

namespace IndustrialMachinery.Services
{
    public class MachineDataServices : IMachineDataServices
    {
        private readonly HttpClient httpClient;

        public MachineDataServices(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Machine> AddMachine(Machine machine)
        {
            //var machineJson =
            //    new StringContent(JsonSerializer.Serialize(machine), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsJsonAsync("/api/machines", machine);

            if (response.IsSuccessStatusCode)
            {
                // return await JsonSerializer.DeserializeAsync<Machine>(await response.Content.ReadAsStreamAsync());
                var m = await response.Content.ReadFromJsonAsync<Machine>();
                return m;
            }

            return null;
        }
        public async Task<Machine> AddUpdatedMachine(Machine machine)
        {
            //var machineJson =
            //    new StringContent(JsonSerializer.Serialize(machine), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsJsonAsync("/api/updatedmachines",machine);

            if (response.IsSuccessStatusCode)
            {
                // return await JsonSerializer.DeserializeAsync<Machine>(await response.Content.ReadAsStreamAsync());
                var m = await response.Content.ReadFromJsonAsync<Machine>();
                return m;
            }

            return null;
        }
        public async Task DeleteMachine(Guid Id)
        {
            await httpClient.DeleteAsync($"api/machines/{Id}");
        }

        public async Task<List<Machine>> GetAllMachines()
        {
            //return await JsonSerializer.DeserializeAsync<IEnumerable<Machine>>
            //        (await httpClient.GetStreamAsync($"api/machines"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
           //return await httpClient.GetFromJsonAsync<IEnumerable<Machine>>("api/machines");
            return await httpClient.GetFromJsonAsync<List<Machine>>("api/machines");

        }

        public async Task<Machine> GetMachineDetails(Guid Id)
        {
                   return await JsonSerializer.DeserializeAsync<Machine>
            (await httpClient.GetStreamAsync($"api/machines/{Id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            //return await httpClient.GetFromJsonAsync<Machine>("api/machines/{Id}");

        }
        public async Task<List<Machine>> GetTemperatures(Guid Id)
        {
            //return await httpClient.GetFromJsonAsync<List<Machine>>("api/updatedmachines/{Id}");
            return await JsonSerializer.DeserializeAsync<List<Machine>>
           (await httpClient.GetStreamAsync($"api/updatedmachines/{Id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });


        }
        public async Task UpdateMachine(Machine machine)
        {

            //var machineJson =
            //    new StringContent(JsonSerializer.Serialize(machine), Encoding.UTF8, "application/json");

            //await httpClient.PutAsync($"api/machines/{machine.Id}", machineJson);
            await httpClient.PutAsJsonAsync<Machine>($"api/machines/{machine.Id}", machine);
        }
    }
}
