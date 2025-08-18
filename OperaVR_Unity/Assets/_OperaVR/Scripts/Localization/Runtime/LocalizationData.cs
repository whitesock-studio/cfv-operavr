using System.Collections.Generic;

namespace OperaVR
{
    [System.Serializable]
    public class LocalizationData
    {
        public List<LanguageTable> LanguagesTables = new();

        public void InitalizeLanguageTables(List<string> laguagesKeys)
        {
            LanguagesTables = new();
            foreach (var key in laguagesKeys)
            {
                LanguagesTables.Add(new LanguageTable(key));
            }
        }

        public bool TryGetValue(string key, out string value)
        {
            var currentLanguage = "it";

            foreach (var languageTable in LanguagesTables)
            {
                if (languageTable.LanguageKey == currentLanguage)
                {
                    return TryGetValue(key, out value);
                }
            }
            value = string.Empty;
            return false;
        }

        public void InsertValue(string languageKey, string key, string value)
        {
            foreach (var languageTable in LanguagesTables)
            {
                if (languageTable.LanguageKey == languageKey)
                {
                    languageTable.InsertValue(key, value);
                    return;
                }
            }
        }
    }
}
