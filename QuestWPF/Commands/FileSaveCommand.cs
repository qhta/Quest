extern alias ExcelTypes;
using ExcelVersion = ExcelTypes::Syncfusion.XlsIO.ExcelVersion;
using ExcelSaveType = ExcelTypes::Syncfusion.XlsIO.ExcelSaveType;

using Path = System.IO.Path;
using IWorkbook = ExcelTypes::Syncfusion.XlsIO.IWorkbook;
namespace QuestWPF;

/// <summary>
/// Command to save a file.
/// A parameter should be of <see cref="FileSaveData"/> type.
/// Handles saving an Excel file as a Quest file.
/// </summary>
public class FileSaveCommand : Command
{
  /// <inheritdoc/>
  public override bool CanExecute(object? parameter)
  {
    if (parameter is FileSaveData data)
    {
      if (data.DataObject is WorkbookInfoVM workbookInfoVM)
        return workbookInfoVM.IsLoaded;

      if (data.DataObject is ProjectQualityVM)
        return true;

    }
    return false;
  }

  /// <inheritdoc/>
  public override async void Execute(object? parameter)
  {
    try
    {
      if (parameter is FileSaveData data)
      {
        if (data.DataObject is WorkbookInfoVM workbookInfoVM && workbookInfoVM.IsLoaded)
{          await SaveWorkbook(workbookInfoVM, data.Filename ?? workbookInfoVM.FileName, data.SaveAs);}
        else
        if (data.DataObject is ProjectQualityVM projectQualityVM)
          await SaveProjectQuality(projectQualityVM, data.Filename ?? projectQualityVM.FileName, data.SaveAs);
      }
    }
    catch (Exception e)
    {
      MessageBox.Show(e.Message);
    }
  }

  private async Task SaveWorkbook(WorkbookInfoVM workbookInfo, string? filename, bool saveAs)
  {
    IWorkbook workbook = (IWorkbook)workbookInfo.Workbook!;

    if (filename == null || saveAs)
    {
      if (filename == null)
        filename = workbookInfo.FileName;
      var fileTypes = new[] { FilenameTools.MakeFilterString(Strings.ExcelXlsFiles, ".xls"), 
                              FilenameTools.MakeFilterString(Strings.ExcelXlsxFiles, ".xlsx"),
                              FilenameTools.MakeFilterString(Strings.ExcelXlsmFiles, ".xlsm"),
      };
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
        workbook.Save();
      });
    }
    else 
    {
      var ext = Path.GetExtension(filename).ToLowerInvariant();
      ExcelSaveType excelSaveType;
      if (ext == ".xlsx")
      {
        if (workbook.Version == ExcelVersion.Excel97to2003)
          workbook.Version = ExcelVersion.Excel2016;
        excelSaveType = ExcelSaveType.SaveAsXLS;
      }      
      else if (ext == ".xlsm")
      {
        if (workbook.Version == ExcelVersion.Excel97to2003)
            workbook.Version = ExcelVersion.Excel2016;
        excelSaveType = ExcelSaveType.SaveAsMacro;
      } 
      else if (ext == ".xls")
      {
        workbook.Version = ExcelVersion.Excel97to2003;
        excelSaveType = ExcelSaveType.SaveAsXLS;
      } 
      else
        throw new InvalidOperationException($"Invalid file extension");

      await using (var fileStream = File.Create(filename))
        workbook.SaveAs(fileStream, excelSaveType);
      workbookInfo.FileName = filename;
      workbookInfo.ProjectTitle = filename;

    }
  }

  private async Task SaveProjectQuality(ProjectQualityVM projectQuality, string? filename, bool saveAs)
  {
    if (filename == null || saveAs)
    {
      if (filename == null)
        filename = projectQuality.FileName;
      var fileTypes = new[] { FilenameTools.MakeFilterString(Strings.QuestXmlFiles, ".xml"),
                              FilenameTools.MakeFilterString(Strings.QuestZipFiles, ".zip")
      };
      //var ext = Path.GetExtension(filename)?.ToLowerInvariant();
      int filterIndex = 1;
      var saveFileDialog = new SaveFileDialog
      {
        Title = Strings.SaveQuestFileAs,
        FileName = filename ?? String.Empty,
        Filter = String.Join("|", fileTypes),
        FilterIndex = filterIndex,
      };
      if (saveFileDialog.ShowDialog() == true)
        filename = saveFileDialog.FileName;
      else
        return;
    }

    if (!String.IsNullOrEmpty(filename))              
    {
      var ext = Path.GetExtension(filename).ToLowerInvariant();
      var bytes = ext.EndsWith("xml") ?
                    await FileCommandHelper.SerializeProjectAsync(projectQuality.Model):
                    await FileCommandHelper.PackProjectAsync(projectQuality.Model);

      await using (var writer = new StreamWriter(filename))
      {

        writer.BaseStream.Write(bytes, 0, bytes.Length);
      }
    }
  }


}