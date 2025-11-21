
using Microsoft.Win32;

using QuestIMP;

using QuestWPF.Views;

using Syncfusion.XlsIO;

using Path = System.IO.Path;

namespace QuestWPF;

/// <summary>
/// Command to save a file.
/// A parameter should be of <see cref="IFileSaveData"/> type.
/// Handles saving an Excel file as a Quest file.
/// </summary>
public class FileSaveCommand : Command
{
  /// <inheritdoc/>
  public override bool CanExecute(object? parameter)
  {
    if (parameter is IFileSaveData data)
    {
      if (data.DataObject is WorkbookInfoVM workbookInfoVM)
        return workbookInfoVM.IsLoaded;

      if (data.DataObject is ProjectQualityVM)
      {
        return true;
      }
    }
    return false;
  }

  /// <inheritdoc/>
  public override async void Execute(object? parameter)
  {
    try
    {
      if (parameter is IFileSaveData data)
      {
        if (data.DataObject is WorkbookInfoVM workbookInfoVM)
          await SaveWorkbook(workbookInfoVM, data.Filename, data.UseDialog);
        else
        if (data.DataObject is ProjectQualityVM projectQualityVM)
          await SaveProjectQuality(projectQualityVM.Model, data.Filename, data.UseDialog);
      }

      // Open a file dialog to select an Excel file

    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }

  private async Task SaveWorkbook(WorkbookInfoVM workbookInfo, string? filename, bool saveAs)
  {
    if (workbookInfo.Workbook == null)
      throw new InvalidOperationException("Workbook is not loaded.");
    if (filename == null || saveAs)
    {
      if (filename == null)
        filename = workbookInfo.FileName;
      var fileTypes = new[] { Strings.ExcelXlsxFilesFilter, Strings.ExcelXslmFilesFilter, Strings.ExcelXlsFilesFilter };
      var ext = Path.GetExtension(filename)?.ToLowerInvariant() ?? ".xlsx";
      int filterIndex = 1;
      if (ext == ".xlsx")
        filterIndex = 1;
      else if (ext == ".xlsm")
        filterIndex = 3;
      else if (ext == ".xls")
        filterIndex = 3;

      var saveFileDialog = new SaveFileDialog
      {
        Title = Strings.SaveExcelFileAs,
        FileName = filename ?? String.Empty,
        Filter = String.Join("|", fileTypes),
        FilterIndex = filterIndex,
      };
      if (saveFileDialog.ShowDialog() == true)
        filename = saveFileDialog.FileName;
      else
        return;
    }

    if (filename == workbookInfo.FileName)
    {
      await Task.Run(() =>
      {
        workbookInfo.Workbook.Save();
      });
    }
    else 
    {
      var ext = Path.GetExtension(filename).ToLowerInvariant();
      ExcelSaveType excelSaveType;
      if (ext == ".xlsx")
      {
        if (workbookInfo.Workbook.Version == ExcelVersion.Excel97to2003)
          workbookInfo.Workbook.Version = ExcelVersion.Excel2016;
        excelSaveType = ExcelSaveType.SaveAsXLS;
      }      
      else if (ext == ".xlsm")
      {
        if (workbookInfo.Workbook.Version == ExcelVersion.Excel97to2003)
          workbookInfo.Workbook.Version = ExcelVersion.Excel2016;
        excelSaveType = ExcelSaveType.SaveAsMacro;
      } 
      else if (ext == ".xls")
      {
        workbookInfo.Workbook.Version = ExcelVersion.Excel97to2003;
        excelSaveType = ExcelSaveType.SaveAsXLS;
      } 
      else
        throw new InvalidOperationException($"Invalid file extension");

      await using (var fileStream = File.Create(filename))
        workbookInfo.Workbook.SaveAs(fileStream, excelSaveType);
      workbookInfo.FileName = filename;
      workbookInfo.ProjectTitle = filename;

    }
  }

  private async Task SaveProjectQuality(ProjectQuality projectQuality, string? filename, bool saveAs)
  {
    if (filename == null || saveAs)
    {
      var fileTypes = new[] { Strings.QuestFilesFilter, Strings.AllFilesFilter };
      var saveFileDialog = new SaveFileDialog
      {
        FileName = filename ?? String.Empty,
        Filter = String.Join("|", fileTypes),
        Title = Strings.SaveQuestFileAs
      };
      if (saveFileDialog.ShowDialog() == true)
        filename = saveFileDialog.FileName;
    }

    if (filename != null)
    {
      await using (var writer = new StreamWriter(filename))
      {
        var xmlSerializer = new Qhta.Xml.Serialization.QXmlSerializer(typeof(ProjectQuality));
        xmlSerializer.Serialize(writer, projectQuality);
      }
    }
  }


}