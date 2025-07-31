
using UnityEngine;

namespace OperaVR
{
    public class MultitrackPopup : APopup
    {
        [SerializeField]
        private MultitrackContentController _multitrackContentController;

        public MultitrackPopupData MultitrackData => Data as MultitrackPopupData;

        protected override void OnPreOpened()
        {
            base.OnPreOpened();
            _multitrackContentController.LoadData(MultitrackData);
        }
    }
}