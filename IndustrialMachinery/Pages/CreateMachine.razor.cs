using IndustrialMachinery.Core;
using IndustrialMachinery.Services;
using Microsoft.AspNetCore.Components;
#nullable disable
namespace IndustrialMachinery.Pages
{
    public partial class CreateMachine
    {
        [CascadingParameter]
        public IndustrialMachinery.Shared.MainLayout MainLayout { get; set; }

        [Inject]
        public IMachineDataServices MachineDataService { get; set; }

        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public EventCallback<bool> refresh { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public Machine Machine { get; set; } = new Machine();


        //protected override async Task OnInitializedAsync()
        //{
        //}

        protected async Task HandleValidSubmit()
        {
            await MachineDataService.AddMachine(Machine);
            NavigationManager.NavigateTo("/");
        }
        protected void HandleInvalidSubmit()
        {
           
        }
        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
