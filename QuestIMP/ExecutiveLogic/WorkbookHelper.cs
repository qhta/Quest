namespace QuestIMP;


/// <summary>
/// Class with helper methods for workbook processing.
/// </summary>
public static class WorkbookHelper
{
  /// <summary>
  /// GetCellColumnIndex - returns zero based column index for the given cell address.
  /// </summary>
  /// <param name="cellAddress"></param>
  /// <returns></returns>
  public static int GetCellRowIndex(string cellAddress)
  {
    return int.Parse(new String(cellAddress.Where(Char.IsDigit).ToArray())) - 1;
  }
}