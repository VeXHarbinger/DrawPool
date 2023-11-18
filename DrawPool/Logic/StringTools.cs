using WPFLocalizeExtension.Engine;

namespace DrawPool.Logic
{
    /// <summary>
    /// General String manipulation Tools
    /// </summary>
    public class StringTools
    {
        /// <summary>
        /// Gets the localized version of the given string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The localized string</returns>
        public static string GetLocalized(string key) => LocalizeDictionary.Instance.GetLocalizedObject("DrawPool", "DrawPoolStrings", key, LocalizeDictionary.Instance.Culture)?.ToString();
    }
}