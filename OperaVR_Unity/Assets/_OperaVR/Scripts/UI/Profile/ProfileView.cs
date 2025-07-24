using System;
using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class ProfileView : MonoBehaviour
    {
        public Action OnClose;

        [SerializeField]
        private Button _closeButton;

        private void Awake()
        {
            _closeButton.onClick.AddListener(() => OnClose?.Invoke());
        }

        public void LoadProfile(ProfileData data)
        {

        }
    }
}
