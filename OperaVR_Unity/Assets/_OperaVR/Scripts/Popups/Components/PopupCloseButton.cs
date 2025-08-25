using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    [RequireComponent(typeof(Button))]
    public class PopupCloseButton : MonoBehaviour
    {
        private Button _button;
        private APopup _popup;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ClosePopup);

            _popup = GetComponentInParent<APopup>();
            _button.interactable = _popup != null;
        }

        private void ClosePopup()
        {
            if (_popup == null)
            {
                return;
            }

            if (_popup.IsComplete)
            {
                _popup.OnSuccess?.Invoke(_popup);
            }
            PopupsManager.Instance.ClosePopup(_popup);
        }
    }
}