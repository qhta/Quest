using Microsoft.JSInterop;

namespace QuestWASM;

/// <summary>
/// Service to handle file uploads in Blazor WebAssembly
/// </summary>
public class FileUploadService
{
    private readonly IJSRuntime _jsRuntime;

    public FileUploadService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Opens a file picker and returns the selected file content
    /// </summary>
    /// <param name="accept">File type filter (e.g., ".quest,.xml")</param>
    /// <returns>Tuple of filename and file content, or null if cancelled</returns>
    public async Task<(string FileName, byte[] Content)?> OpenFileAsync(string accept = ".qxml,.quest")
    {
        try
        {
            var result = await _jsRuntime.InvokeAsync<FileData?>("openFileDialog", accept);
            if (result != null && !string.IsNullOrEmpty(result.FileName))
            {
                var bytes = Convert.FromBase64String(result.Base64Content);
                return (result.FileName, bytes);
            }
            return null;
        }
        catch (JSException)
        {
            return null;
        }
    }

    /// <summary>
    /// Data structure for file upload results
    /// </summary>
    public class FileData
    {
        public string FileName { get; set; } = string.Empty;
        public string Base64Content { get; set; } = string.Empty;
    }
}