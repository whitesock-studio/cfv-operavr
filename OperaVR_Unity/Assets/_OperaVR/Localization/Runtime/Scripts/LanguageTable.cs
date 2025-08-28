using System.Collections.Generic;

namespace OperaVR
{
    [System.Serializable]
    public class LanguageTable
    {
        public LanguageTable(string languageKey)
        {
            LanguageKey = languageKey;
        }

        public string LanguageKey;
        public List<LocalizationKeyValuePair> Pairs = new();

        public bool TryGetValue(string key, out string value)
        {
            foreach (var pair in Pairs)
            {
                if (pair.Key == key)
                {
                    value = pair.Value;
                    return true;
                }
            }
            value = string.Empty;
            return false;
        }

        public void InsertValue(string key, string value)
        {
            foreach (var pair in Pairs)
            {
                if (pair.Key == key)
                {
                    pair.Value = value;
                    return;
                }
            }
            Pairs.Add(new LocalizationKeyValuePair(key, value));
        }
    }

    [System.Serializable]
    public class LocalizationKeyValuePair
    {
        public string Key;
        public string Value;

        public LocalizationKeyValuePair(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
