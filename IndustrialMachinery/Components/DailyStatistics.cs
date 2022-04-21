using IndustrialMachinery.Core;
using IndustrialMachinery.Services;
using Microsoft.AspNetCore.Components;
#nullable disable

namespace IndustrialMachinery.Components
{
    public  partial class DailyStatistics
    {
        public IEnumerable<Machine> Machines { get; set; }
        public int TotalMachines { get; set; }
        public int TotalMachinesOn { get; set; }
        [Inject]
        public IMachineDataServices MachineDataService { get; set; }
        protected async override Task OnInitializedAsync()
        {
            Machines = (await MachineDataService.GetAllMachines()).ToList();

            TotalMachines = Machines.Count();
            TotalMachinesOn = Machines.Where(m => m.Status is true).Count();


        }

    }
}
