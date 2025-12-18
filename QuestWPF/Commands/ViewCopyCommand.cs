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
          var bytes = await FileCommandHelper.SerializeProjectAsync(projectQualityVM.Model);
          Clipboard.SetData(DataFormats.Serializable, bytes);
          var memoryStream = new MemoryStream(bytes);
          using (var reader = new StreamReader(memoryStream))
          {
            var str = reader.ReadToEnd();
            Clipboard.SetText(str);
          }
        }
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
  public async void CopyDocumentQuestView(DocumentQuestView view)
  {
    DocumentQuality? model = (view.DataContext as DocumentQualityVM)?.Model;
    if (model == null)
      return;
    try
    {
      //var serializedData = await FileCommandHelper.SerializeProjectAsync(model);
      //Clipboard.SetData(DataFormats.Serializable, serializedData);

    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }

}
