using IndustrialMachinery.Core;
using IndustrialMachinery.Services;
using Microsoft.AspNetCore.Components;
#nullable disable
namespace IndustrialMachinery.Pages
{
    public partial class MachineDelete
    {
        [Parameter]
        public string Id { get; set; }

        public Machine Machine { get; set; } = new Machine();

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IMachineDataServices MachineDataService { get; set; }
        protected async override Task OnInitializedAsync()
        {
            Machine = await MachineDataService.GetMachineDetails(Guid.Parse(Id));
        }
        protected async Task DeleteEmployee()
        {
            await MachineDataService.DeleteMachine(Machine.Id);
            NavigationManager.NavigateTo("/");

        }
    }
}
