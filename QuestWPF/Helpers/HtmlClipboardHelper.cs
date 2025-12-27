namespace QuestWPF.Helpers;

/// <summary>
/// Helper methods for HTML Clipboard Format.
/// </summary>
public static class HtmlClipboardHelper
{
  /// <summary>
  /// Converts HTML content to the HTML Clipboard Format required by Microsoft Office applications.
  /// </summary>
  /// <param name="html">The HTML content to convert.</param>
  /// <returns>HTML content with proper clipboard format headers.</returns>
  public static string ConvertToHtmlClipboardFormat(string html)
  {
    // Encode to UTF-8
    byte[] utf8Bytes = Encoding.UTF8.GetBytes(html);
    string utf8Html = Encoding.UTF8.GetString(utf8Bytes);

    // Build the clipboard format header
    StringBuilder sb = new StringBuilder();

    // Version
    sb.AppendLine("Version:0.9");

    // Calculate byte positions (header + content)
    string header = sb.ToString();
    int startHTML = header.Length + 100; // Approximate header size
    int endHTML = startHTML + utf8Bytes.Length;

    // Find fragment markers
    int startFragment = utf8Html.IndexOf("<!--StartFragment-->", StringComparison.Ordinal);
    int endFragment = utf8Html.IndexOf("<!--EndFragment-->", StringComparison.Ordinal);

    if (startFragment >= 0)
      startFragment = startHTML + startFragment + "<!--StartFragment-->".Length;
    else
      startFragment = startHTML;

    if (endFragment >= 0)
      endFragment = startHTML + endFragment;
    else
      endFragment = endHTML;

    // Build complete header
    sb.Clear();
    sb.AppendLine("Version:0.9");
    sb.AppendLine($"StartHTML:{startHTML:D10}");
    sb.AppendLine($"EndHTML:{endHTML:D10}");
    sb.AppendLine($"StartFragment:{startFragment:D10}");
    sb.AppendLine($"EndFragment:{endFragment:D10}");
    sb.Append(utf8Html);

    return sb.ToString();
  }

}