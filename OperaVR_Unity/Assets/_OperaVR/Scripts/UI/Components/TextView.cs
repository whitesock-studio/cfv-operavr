using TMPro;
using UnityEngine;

namespace OperaVR
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextView : MonoBehaviour
    {
        public TMP_Text TextDisplayer;

        [SerializeField]
        private Color _normalColor;
        
        [SerializeField]
        private Color _notInteractableColor;

        [SerializeField]
        private Color _hiddenColor;

        private void Awake()
        {
            TextDisplayer.color = _normalColor;
        }

        public void SetSelected(bool isSelected)
        {
            if (isSelected)
            {
                TextDisplayer.color = _normalColor;
            }
        }

        public void SetInteractable(bool isInteractable)
        {
            TextDisplayer.color = isInteractable ? _normalColor : _notInteractableColor;
        }

        public void SetHidden(bool isHidden)
        {
            TextDisplayer.color = isHidden ? _hiddenColor : _normalColor;
        }
    }
}
