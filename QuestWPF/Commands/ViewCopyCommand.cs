namespace QuestWPF;

/// <summary>
/// Command to copy current view to clipboard.
/// </summary>
public class ViewCopyCommand : Command
{
  /// <summary>
  /// Determines whether the command can execute with the specified parameter.
  /// </summary>
  public override bool CanExecute(object? parameter)
  {
    parameter = (parameter as ViewCopyData)?.ActiveView;
    if (parameter is QuestView questView)
    {
      var projectQualityVM = questView.DataContext as ProjectQualityVM;
      return projectQualityVM != null;
    }
    return false;
  }

  /// <summary>
  /// A method to execute the command. The parameter should be a reference to current QuestView.
  /// </summary>
  /// <param name="parameter"></param>
  public override async void Execute(object? parameter)
  {
    parameter = (parameter as ViewCopyData)?.ActiveView;
    try
    {
      if (parameter is QuestView questView)
      {
        var projectQualityVM = questView.DataContext as ProjectQualityVM;
        if (projectQualityVM != null)
        {
          if (questView.ModelTreeView.SelectedItem is DocumentQualityVM documentQualityVM)
          {
            CopyDocumentQuality(documentQualityVM);
          }
          else
            CopyProjectQuality(projectQualityVM);
        }
      }
    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }
  /// <summary>
  /// Copies the current ProjectQualityVM to clipboard as  serializable and as XML text.
  /// </summary>
  /// <returns></returns>
  public async void CopyProjectQuality(ProjectQualityVM projectQualityVM)
  {
    ProjectQuality? model = projectQualityVM.Model;
    if (model == null)
      return;
    try
    {
      var bytes = await FileCommandHelper.SerializeProjectQualityAsync(model);
      Clipboard.SetData(DataFormats.Serializable, bytes);
      var memoryStream = new MemoryStream(bytes);
      using (var reader = new StreamReader(memoryStream))
      {
        var str = reader.ReadToEnd();
        Clipboard.SetText(str);
      }

    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }

  /// <summary>
  /// Copies the current DocumentQuestView to clipboard as XML text.
  /// </summary>
  /// <returns></returns>
  public async void CopyDocumentQuality(DocumentQualityVM documentQualityVM)
  {
    DocumentQuality? model = documentQualityVM.Model;
    if (model == null)
      return;
    try
    {
      var bytes = await FileCommandHelper.SerializeDocumentQualityAsync(model);
      Clipboard.SetData(DataFormats.Serializable, bytes);
      var memoryStream = new MemoryStream(bytes);
      using (var reader = new StreamReader(memoryStream))
      {
        var str = reader.ReadToEnd();
        Clipboard.SetText(str);
      }

    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }

}
