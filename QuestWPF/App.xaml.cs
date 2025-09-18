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

  /// <summary>
  /// Initializes the application on startup.
  /// </summary>
  /// <param name="e"></param>
  protected override void OnStartup(StartupEventArgs e)
  {
    base.OnStartup(e);
    // Initialize QuestRDM database and seed Projects table if needed
    QuestRDM.QuestRdmDbInitializer.Initialize();
  }
}