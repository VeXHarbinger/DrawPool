using WPFLocalizeExtension.Engine;

namespace DrawPool.Logic
{
    public class Strings
    {
        public static string GetLocalized(string key) => LocalizeDictionary.Instance.GetLocalizedObject("DrawPool", "Strings", key, LocalizeDictionary.Instance.Culture)?.ToString();
    }
}
