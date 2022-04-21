using IndustrialMachinery;
using IndustrialMachinery.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddHttpClient<IMachineDataServices, MachineDataServices>(client => client.BaseAddress = new Uri("https://apifunctionmachinery.azurewebsites.net"));
builder.Services.AddHttpClient<IMachineDataServices, MachineDataServices>(client => client.BaseAddress = new Uri("https://tablestoragefnapi.azurewebsites.net/"));
//builder.Services.AddHttpClient<IMachineDataServices, MachineDataServices>(client => client.BaseAddress = new Uri("http://localhost:7071"));
await builder.Build().RunAsync();
