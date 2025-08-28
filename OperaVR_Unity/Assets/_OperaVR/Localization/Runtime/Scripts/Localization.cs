using System;
using UnityEngine;

namespace OperaVR
{
    public class Localization : MonoBehaviour
    {
        public Action OnLocaleChanged;

        public static Localization Instance;

        public LocalizationData Data;

        public string LanguageKey = "IT";
        public string FallbackLanguageKey = "EN";

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public bool TryGetLocalizedText(string key, out string value)
        {
            if (Data.TryGetValue(key, LanguageKey, out value))
            {
                return true;
            }
            Debug.LogWarning($"Missing key: {key} in language {LanguageKey}");
            if (Data.TryGetValue(key, FallbackLanguageKey, out value))
            {
                return true;
            }
            Debug.LogWarning($"Missing key: {key} in fallback language {LanguageKey}");
            value = string.Empty;
            return false;
        }

        public void SetLocale(string locale)
        {
            LanguageKey = locale;
            OnLocaleChanged?.Invoke();
        }
    }
}
