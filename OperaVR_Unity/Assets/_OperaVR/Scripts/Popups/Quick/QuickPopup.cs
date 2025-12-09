using UnityEngine;

namespace OperaVR
{
    public class QuickPopup : APopup
    {
        public QuickPopupData QuickData => Data as QuickPopupData;

        public override bool IsComplete => Time.time > _closeTime;

        private float _closeTime = 0f;
        
        private void Update()
        {
            if (Time.time < _closeTime)
            {
                return;
            }
            PopupsManager.Instance.ClosePopup(this);
        }

        protected override void OnPreOpened()
        {
            _closeTime = Time.time + QuickData.LogTime;
            base.OnPreOpened();
        }
    }
}