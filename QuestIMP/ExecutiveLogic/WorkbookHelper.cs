using Syncfusion.XlsIO.Implementation.Exceptions;

namespace QuestIMP;


/// <summary>
/// Class with helper methods for workbook processing.
/// </summary>
public static class WorkbookHelper
{
  /// <summary>
  /// Splits a range string into its start and end address.
  /// </summary>
  /// <remarks>This method assumes that the input string is properly formatted as "start:end". If the input does
  /// not meet this format, the behavior may be undefined or an exception may be thrown.</remarks>
  /// <param name="range">A string representing a range, formatted as "start:end", where "start" and "end" are substrings separated by a
  /// colon (:).</param>
  /// <returns>A tuple containing the start and end cell address of the range.</returns>
  public static (string, string) SplitRange(string range)
  {
    var ss = range.Split(':');
    return (ss[0], ss[1]);
  }

  /// <summary>
  /// Returns zero based row index for the given cell address.
  /// </summary>
  /// <param name="cellAddress"></param>
  /// <returns></returns>
  public static int GetCellRowIndex(string cellAddress)
  {
    return int.Parse(new String(cellAddress.Where(Char.IsDigit).ToArray())) - 1;
  }

  /// <summary>
  /// Returns zero based column index for the given cell address.
  /// </summary>
  /// <param name="cellAddress"></param>
  /// <returns></returns>
  public static int GetCellColumnIndex(string cellAddress)
  {
    if (cellAddress.Length < 2)
      throw new InvalidRangeException("Cell address must have at least two characters");
    var ch = cellAddress[0];
    if (ch<'A' || ch>'Z')
      throw new InvalidRangeException("Cell address must start with uppercase letter");
    var result = (int)(ch-'A');
    ch = cellAddress[1];
    if (ch < 'A' || ch > 'Z')
      return result;
    result = (result + 1) * 26 + (int)(ch - 'A');
    return result;
  }
}