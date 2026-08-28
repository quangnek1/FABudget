using Blazored.LocalStorage;
using ItemMaster.Client;
using ItemMaster.Client.Providers;
using ItemMaster.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddHttpClient<FaClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddHttpClient("Itemmaster.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Itemmaster.ServerAPI"));
builder.Services.AddMudServices();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<TokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TokenAuthenticationStateProvider>());
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFaClient, FaClient>();

await builder.Build().RunAsync();