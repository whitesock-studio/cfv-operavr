using UnityEngine;

namespace OperaVR
{
    public class PopupRequester : MonoBehaviour
    {
        [SerializeField]
        private APopupData _data;

        [SerializeField]
        private bool _openAtStart;

        private APopup _openedPopup;
        public APopup OpenedPopup => _openedPopup;

        private void Start()
        {
            if (_openAtStart)
            {
                Open();
            }
        }

        public void Open()
        {
            Close();
            _openedPopup = PopupsManager.Instance.OpenPopup(_data);
        }

        public void Close()
        {
            if (_openedPopup != null)
            {
                PopupsManager.Instance.ClosePopup(_openedPopup);
            }
        }
    }
}