using System.IO.Compression;

using Qhta.Xml.Serialization;

namespace Quest;

/// <summary>
/// Helper class for project quality serialization and deserialization
/// </summary>
public static class FileCommandHelper
{

  /// <summary>
  /// Serializes the project quality to XML format
  /// </summary>
  public static async Task<byte[]> SerializeProjectQualityAsync(ProjectQuality projectQuality)
  {

    return await Task.Run(() =>
    {
      using var memoryStream = new MemoryStream();
      using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
      {
        var xmlSerializer = new QXmlSerializer(typeof(ProjectQuality));
        xmlSerializer.Serialize(writer, projectQuality);
      }
      return memoryStream.ToArray();
    });
  }

  /// <summary>
  /// Deserializes project quality from XML data
  /// </summary>
  public static async Task<ProjectQuality?> DeserializeProjectQualityAsync(byte[] data)
  {
    return await Task.Run(() =>
    {
      using var memoryStream = new MemoryStream(data);
      using var reader = new StreamReader(memoryStream);
      var xmlSerializer = new QXmlSerializer(typeof(ProjectQuality));
      return xmlSerializer.Deserialize(reader) as ProjectQuality;
    });
  }

  /// <summary>
  /// Packs the project quality to XML format inside a ZIP archive
  /// </summary>
  public static async Task<byte[]> PackProjectQualityAsync(ProjectQuality projectQuality)
  {
    var filename = "Quest.xml";
    var bytes = await SerializeProjectQualityAsync(projectQuality);
    return await Task.Run(() =>
    {
      using var outputStream = new MemoryStream();
      using (var zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create, true))
      {
        var entryName = Path.GetFileNameWithoutExtension(filename) + ".xml";
        var zipEntry = zipArchive.CreateEntry(entryName, CompressionLevel.Optimal);

        using var entryStream = zipEntry.Open();
        entryStream.Write(bytes, 0, bytes.Length);
      }
      return outputStream.ToArray();
    });
  }

  /// <summary>
  /// Unpack ZIP archive and deserializes project quality from XML data
  /// </summary>
  public static async Task<ProjectQuality?> UnpackProjectQualityAsync(byte[] data)
  {
    return await Task.Run(() =>
    {
      using var inputStream = new MemoryStream(data);
      using var zipArchive = new ZipArchive(inputStream, ZipArchiveMode.Read);

      var entry = zipArchive.Entries.FirstOrDefault(e => e.Name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase));
      if (entry == null)
        return null;

      using var entryStream = entry.Open();
      using var reader = new StreamReader(entryStream);
      var xmlSerializer = new QXmlSerializer(typeof(ProjectQuality));
      return xmlSerializer.Deserialize(reader) as ProjectQuality;
    });
  }

  /// <summary>
  /// Serializes the document quality to XML format
  /// </summary>
  public static async Task<byte[]> SerializeDocumentQualityAsync(DocumentQuality documentQuality)
  {

    return await Task.Run(() =>
    {
      using var memoryStream = new MemoryStream();
      using (var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
      {
        var xmlSerializer = new QXmlSerializer(typeof(DocumentQuality));
        xmlSerializer.Serialize(writer, documentQuality);
      }
      return memoryStream.ToArray();
    });
  }

  /// <summary>
  /// Deserializes document quality from XML data
  /// </summary>
  public static async Task<DocumentQuality?> DeserializeDocumentQUalityAsync(byte[] data)
  {
    return await Task.Run(() =>
    {
      using var memoryStream = new MemoryStream(data);
      using var reader = new StreamReader(memoryStream);
      var xmlSerializer = new QXmlSerializer(typeof(DocumentQuality));
      return xmlSerializer.Deserialize(reader) as DocumentQuality;
    });
  }
}