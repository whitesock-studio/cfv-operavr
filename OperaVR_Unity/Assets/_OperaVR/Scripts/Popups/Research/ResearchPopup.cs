using UnityEngine;

namespace OperaVR
{
    public class ResearchPopup : APopup
    {
        [SerializeField]
        private ResearchContentController _researchContentController;

        public ResearchPopupData ResearchData => Data as ResearchPopupData;

        public override bool IsComplete => _researchContentController.CanComplete();

        private void Awake()
        {
            _researchContentController.OnComplete += OnContentCompleted;
            _researchContentController.OnFail += () => OnFail?.Invoke(this);
        }

        protected override void OnPreOpened()
        {
            base.OnPreOpened();
            _researchContentController.LoadData(ResearchData);
        }

        private void OnContentCompleted()
        {
            OnSuccess?.Invoke(this);
            PopupsManager.Instance.ClosePopup(this);
        }
    }
}