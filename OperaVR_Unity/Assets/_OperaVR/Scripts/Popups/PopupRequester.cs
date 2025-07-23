using UnityEngine;
using UnityEngine.Events;

namespace OperaVR
{
    public class PopupRequester : MonoBehaviour
    {
        public UnityEvent OnPopupClosed;
        public UnityEvent OnPopupSucceded;
        public UnityEvent OnPopupFail;

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
            _openedPopup.OnClose += PopupClosed;
            _openedPopup.OnSuccess += PopupSucceded;
            _openedPopup.OnFail += PopupFailed;
        }

        public void Close()
        {
            if (_openedPopup != null)
            {
                PopupsManager.Instance.ClosePopup(_openedPopup);
            }
        }

        private void PopupClosed(APopup popup)
        {
            popup.OnClose -= PopupClosed;
            popup.OnSuccess -= PopupSucceded;
            popup.OnFail -= PopupFailed;
            OnPopupClosed?.Invoke();
            Debug.Log(popup + " closed");
        }

        private void PopupSucceded(APopup popup)
        {
            OnPopupSucceded?.Invoke();
            Debug.Log(popup + " succeded");
        }

        private void PopupFailed(APopup popup)
        {
            OnPopupFail?.Invoke();
            Debug.Log(popup + " failed");
        }
    }
}