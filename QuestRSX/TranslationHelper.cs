using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;

using Azure;
using Azure.AI.Translation.Text;
using Azure.Identity;

namespace QuestRSX;

/// <summary>
/// Helper class for text translation.
/// </summary>
public static class TranslationHelper
{
  private static TextTranslationClient? TranslationClient;

  /// <summary>
  /// Translates the specified text from the source language to the target language.
  /// </summary>
  /// <remarks>This method uses the Azure Cognitive Services Translator API to perform the translation. The
  /// detected language of the input text and the confidence score are logged for debugging purposes.</remarks>
  /// <param name="inputText">The text to be translated. Cannot be null or empty.</param>
  /// <param name="sourceLanguage">The language code of the source text (e.g., "en" for English). This parameter is currently unused and may be
  /// ignored.</param>
  /// <param name="targetLanguage">The language code of the target language (e.g., "pl" for Polish). Must be a valid language code supported by the
  /// translation service.</param>
  /// <returns>The translated text as a string. Returns an empty string if the translation fails or no translation is available.</returns>
  public static string? TranslateText(string inputText, string sourceLanguage, string targetLanguage)
  {
    try
    {
      if (TranslationClient == null)
      {
        string endpoint = "https://api.cognitive.microsofttranslator.com/";
        DefaultAzureCredential credential = new DefaultAzureCredential();
        TranslationClient = new TextTranslationClient(credential, new Uri(endpoint));
      }

      Response<IReadOnlyList<TranslatedTextItem>> response = TranslationClient.Translate(targetLanguage, inputText);
      IReadOnlyList<TranslatedTextItem> translations = response.Value;
      TranslatedTextItem? translation = translations.FirstOrDefault();

      Debug.WriteLine($"Detected languages of the input text: {translation?.DetectedLanguage?.Language} with score: {translation?.DetectedLanguage?.Confidence}.");
      Debug.WriteLine($"Text was translated to: '{translation?.Translations?.FirstOrDefault()?.TargetLanguage}' and the result is: '{translation?.Translations?.FirstOrDefault()?.Text}'.");
      return translation?.Translations?.FirstOrDefault()?.Text ?? string.Empty;
    }
    catch (RequestFailedException exception)
    {
      Debug.WriteLine($"Error Code: {exception.ErrorCode}");
      Debug.WriteLine($"Message: {exception.Message}");
      return null;
    }
  }
}