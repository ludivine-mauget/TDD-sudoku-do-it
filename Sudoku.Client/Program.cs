using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sudoku.Client;
using Sudoku.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5050") });

builder.Services.AddScoped<ISudokuApiClient, SudokuApiClient>();
builder.Services.AddSingleton<GameStateService>();

await builder.Build().RunAsync();
