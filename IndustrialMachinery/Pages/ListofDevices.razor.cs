using IndustrialMachinery.Core;
using IndustrialMachinery.Services;
using Microsoft.AspNetCore.Components;
#nullable disable
namespace IndustrialMachinery.Pages
{
    public partial class ListofDevices
    {
        public string Message { get; set; }
        public List<Machine> Machines { get; set; }

        //[CascadingParameter]
        //public IndustrialMachinery.Shared.MainLayout MainLayout { get; set; }

        [Inject]
        public IMachineDataServices MachineDataService { get; set; }


        protected async override Task OnInitializedAsync()
        {

            Machines = (await MachineDataService.GetAllMachines()).ToList();

        }
        private async Task Machinestatus(Machine machine)
        {
            machine.Status = machine.Status ? false : true;
            await MachineDataService.UpdateMachine(machine);
        }

        private async Task UpdateMachine(Machine machine)
        {
            await MachineDataService.AddUpdatedMachine(machine);
            Machines = (await MachineDataService.GetAllMachines()).ToList();
            Message = "Temperature Updated Successfully";
            
        }

        
    }
}
