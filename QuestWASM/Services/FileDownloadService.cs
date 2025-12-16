using Microsoft.JSInterop;

namespace QuestWASM;

/// <summary>
/// Service to handle file downloads in Blazor WebAssembly
/// </summary>
public class FileDownloadService
{
    private readonly IJSRuntime _jsRuntime;

    public FileDownloadService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Downloads a file to the user's browser
    /// </summary>
    /// <param name="fileName">Suggested file name</param>
    /// <param name="contentType">MIME type</param>
    /// <param name="data">File content as byte array</param>
    public async Task SaveAsAsync(string fileName, string contentType, byte[] data)
    {
        var base64 = Convert.ToBase64String(data);
        await _jsRuntime.InvokeVoidAsync("downloadFile", fileName, contentType, base64);
    }
}