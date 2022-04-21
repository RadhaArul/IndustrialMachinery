using IndustrialMachinery.Core;
using IndustrialMachinery.Services;
using Microsoft.AspNetCore.Components;
#nullable disable

namespace IndustrialMachinery.Pages
{
    public partial class MachineEdit
    {

        [Inject]
        public IMachineDataServices MachineDataService { get; set; }

        [Parameter]
        public string Id { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        
        public Machine machine { get; set; } = new Machine();


        protected override async Task OnInitializedAsync()
        {
            machine = await MachineDataService.GetMachineDetails(Guid.Parse(Id));
            machine.Id = Guid.Parse(Id);
        }

        protected async Task HandleValidSubmit()
        {
                await MachineDataService.UpdateMachine(machine);
            NavigationManager.NavigateTo("/");
            //NavigationManager.NavigateTo("/");
        }

        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/");
            //NavigationManager.NavigateTo("/");
        }
        protected void HandleInvalidSubmit()
        {

        }

    }
}
