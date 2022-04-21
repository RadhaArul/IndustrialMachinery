using IndustrialMachinery.Core;
using IndustrialMachinery.Services;
using Microsoft.AspNetCore.Components;
#nullable disable

namespace IndustrialMachinery.Pages
{
    public partial class MachineDetail
    {

        [Parameter]
        public string Id { get; set; }

        public Machine Machine { get; set; } = new Machine();
        public List<Machine> Temperatures { get; set; }   


        [Inject]
        public IMachineDataServices MachineDataService { get; set; }
        protected async override Task OnInitializedAsync()
        {
            Machine = await MachineDataService.GetMachineDetails(Guid.Parse(Id));
            Temperatures = await MachineDataService.GetTemperatures(Guid.Parse(Id));

        }

    }
}
