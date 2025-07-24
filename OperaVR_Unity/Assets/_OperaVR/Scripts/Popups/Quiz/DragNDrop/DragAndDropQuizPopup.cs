using UnityEngine;

namespace OperaVR
{
    public class DragAndDropQuizPopup : APopup
    {
        [SerializeField]
        private DragAndDropContentController _dragAndDropContentController;

        public DragAndDropQuizPopupData DragAndDropData => Data as DragAndDropQuizPopupData;


        private void Awake()
        {
            _dragAndDropContentController.OnComplete += OnContentCompleted;
            _dragAndDropContentController.OnFail += () => OnFail?.Invoke(this);
        }

        protected override void OnPreOpened()
        {
            base.OnPreOpened();
            _dragAndDropContentController.LoadData(DragAndDropData);
        }

        private void OnContentCompleted()
        {
            OnSuccess?.Invoke(this);
            PopupsManager.Instance.ClosePopup(this);
        }
    }
}