using Quest;

using QuestWPF.Helpers;
using QuestWPF.Views;

using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;

using ColorConverter = System.Windows.Media.ColorConverter;
using GridColumn = Syncfusion.UI.Xaml.Grid.GridColumn;

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
    if (parameter is ViewCopyData ViewCopyData)
      parameter = ViewCopyData.ActiveView;

    if (parameter is DocumentQuestView)
      return true;
    if (parameter is DocumentQuestResultsView)
      return true;

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
  public override void Execute(object? parameter)
  {
    if (parameter is ViewCopyData ViewCopyData)
      parameter = ViewCopyData.ActiveView;

    try
    {
      if (parameter is DocumentQuestView documentQuestView)
      {
        CopyDocumentQuestView(documentQuestView);
      }
      else if (parameter is DocumentQuestResultsView documentQuestResultsView)
      {
        CopyDocumentQualityResultsView(documentQuestResultsView);
      }
      else if (parameter is QuestView questView)
      {
        if (questView.DataContext is ProjectQualityVM projectQualityVM)
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
  public void CopyProjectQuality(ProjectQualityVM projectQualityVM)
  {
    ProjectQuality? model = projectQualityVM.Model;
    try
    {
      DataObject dataObject = new();
      GetProjectQualityData(model, dataObject);
      Clipboard.SetDataObject(dataObject, true);
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
  public void CopyDocumentQuality(DocumentQualityVM documentQualityVM)
  {
    DocumentQuality? model = documentQualityVM.Model;
    DataObject dataObject = new();
    GetDocumentQualityData(model, dataObject);
    Clipboard.SetDataObject(dataObject, true);
  }


  /// <summary>
  /// Copies the current DocumentQuestView to clipboard as XML text.
  /// </summary>
  /// <returns></returns>
  public void CopyDocumentQuestView(DocumentQuestView documentQuestView)
  {
    var grid = documentQuestView.DocumentQuestGrid;
    DataObject dataObject = new();
    DataGridReflectorHelper.GetGridAsText(grid, dataObject);
    DataGridReflectorHelper.GetGridAsHtml(grid, dataObject);
    Clipboard.SetDataObject(dataObject, true);
  }

  /// <summary>
  /// Copies the current DocumentQuestView to clipboard as XML text.
  /// </summary>
  /// <returns></returns>
  public void CopyDocumentQualityResultsView(DocumentQuestResultsView documentQuestResultsView)
  {
    var grid = documentQuestResultsView.DocumentQuestResultsGrid;
    DataObject dataObject = new();
    GetFrameworkElementAsImage(grid, dataObject);
    DataGridReflectorHelper.GetGridAsText(grid, dataObject);
    DataGridReflectorHelper.GetGridAsHtml(grid, dataObject);
    Clipboard.SetDataObject(dataObject, true);
  }

  /// <summary>
  /// Gets the ProjectQuality data in serializable and XML text formats.
  /// Fills the provided DataObject with the data.
  /// </summary>
  /// <param name="model"></param>
  /// <param name="data"></param>
  private void GetProjectQualityData(ProjectQuality model, DataObject data)
  {
    var bytes = FileCommandHelper.SerializeProjectQualityAsync(model).Result;
    data.SetData(DataFormats.Serializable, bytes);
    var memoryStream = new MemoryStream(bytes);
    using (var reader = new StreamReader(memoryStream))
    {
      var str = reader.ReadToEnd();
      data.SetText(str);
    }
  }

  /// <summary>
  /// Gets the DocumentQuality data in serializable and XML text formats.
  /// Fills the provided DataObject with the data.
  /// </summary>
  /// <param name="model"></param>
  /// <param name="data"></param>
  private void GetDocumentQualityData(DocumentQuality model, DataObject data)
  {
    var bytes = FileCommandHelper.SerializeDocumentQualityAsync(model).Result;
    data.SetData(DataFormats.Serializable, bytes);
    var memoryStream = new MemoryStream(bytes);
    using (var reader = new StreamReader(memoryStream))
    {
      var str = reader.ReadToEnd();
      data.SetText(str);
    }
  }

  /// <summary>
  /// Gets the image representation of a FrameworkElement.
  /// Fills the provided DataObject with the image data.
  /// </summary>
  /// <param name="element"></param>
  /// <param name="data"></param>
  private void GetFrameworkElementAsImage(FrameworkElement element, DataObject data)
  {
    double width = element.ActualWidth;
    double height = element.ActualHeight;
    RenderTargetBitmap bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
    DrawingVisual dv = new DrawingVisual();
    using (DrawingContext dc = dv.RenderOpen())
    {
      dc.DrawRectangle(Brushes.White, null, new Rect(new Point(), new Size(width, height)));
      var vb = new VisualBrush(element);
      dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(width, height)));
    }
    bmpCopied.Render(dv);
    data.SetImage(bmpCopied);
  }

}
