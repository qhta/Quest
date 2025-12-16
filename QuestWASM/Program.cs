using System.Diagnostics;
using System.Globalization;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;


namespace QuestWASM;

public class Program
{
  public static async Task Main(string[] args)
  {
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
    builder.Services.AddSingleton<ProjectQualityService>();

    // Add localization services
    builder.Services.AddLocalization();

    var host = builder.Build();

    // Set culture from browser or user preference
    await SetCultureAsync(host.Services);

    await host.RunAsync();
  }

  private static async Task SetCultureAsync(IServiceProvider services)
  {
    try
    {
      var js = services.GetRequiredService<IJSRuntime>();

      // Use a more defensive approach
      var culture = "pl-PL"; // default

      try
      {
        var storedCulture = await js.InvokeAsync<string>("blazorCulture.get");
        if (!string.IsNullOrEmpty(storedCulture))
          culture = storedCulture;
      }
      catch (JSException jsEx)
      {
        Console.WriteLine($"JS Error getting culture: {jsEx.Message}");
      }

      var cultureInfo = new CultureInfo(culture);
      CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
      CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error setting culture: {ex.Message}");
      // Fallback to default culture
      var defaultCulture = new CultureInfo("en-US");
      CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
      CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;
    }
  }
}
