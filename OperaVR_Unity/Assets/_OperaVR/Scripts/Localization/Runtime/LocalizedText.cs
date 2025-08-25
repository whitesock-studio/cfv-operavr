using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace OperaVR
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField]
        private string _key;
        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                Translate();
            }
        }

        [SerializeField]
        private TMP_Text _textDisplayer;
        public TMP_Text TextDisplayer => _textDisplayer;

        private void Start()
        {
            var localization = Localization.Instance;
            if (localization == null)
            {
                Debug.LogWarning("Missing localization");
                return;
            }
            localization.OnLocaleChanged += Translate;
            Translate();
        }

        private void OnEnable()
        {
            var localization = Localization.Instance;
            if (localization == null)
            {
                return;
            }
            Translate();
        }

        public void Translate()
        {
            if (Localization.Instance.TryGetLocalizedText(Key, out var localizedString))
            {
                _textDisplayer.text = localizedString;
            }
        }
    }
}
