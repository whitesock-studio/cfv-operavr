using UnityEngine;

namespace OperaVR
{
    public class ResearchPopup : APopup
    {
        [SerializeField]
        private ResearchContentController _researchContentController;

        public ResearchPopupData ResearchData => Data as ResearchPopupData;


        private void Awake()
        {
            _researchContentController.OnComplete += OnContentCompleted;
        }

        protected override void OnPreOpened()
        {
            base.OnPreOpened();
            _researchContentController.LoadData(ResearchData);
        }

        private void OnContentCompleted()
        {
            PopupsManager.Instance.ClosePopup(this);
        }
    }
}