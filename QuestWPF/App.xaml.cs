namespace QuestWPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
  /// <summary>
  /// Sets up the application and registers the Syncfusion license key.
  /// </summary>
  public App()
  {
    // Register the Syncfusion license key
    SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cXGpCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZfdXRQQmlYWUB+WERWYEg=");
    //Syncfusion.Diagnostics.DebugLogger.EnableLogging = true;
  }

  //protected override void OnStartup(StartupEventArgs e)
  //{
  //  base.OnStartup(e);
  //}
}