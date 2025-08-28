using System.Collections.Generic;
using UnityEngine;

namespace OperaVR
{
    [CreateAssetMenu(menuName = "Localization/Data", fileName = "LocalizationData")]
    public class LocalizationData : ScriptableObject
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

        public bool TryGetValue(string key, string language, out string value)
        {
            foreach (var languageTable in LanguagesTables)
            {
                if (languageTable.LanguageKey == language)
                {
                    return languageTable.TryGetValue(key, out value);
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
