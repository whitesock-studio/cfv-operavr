using System.Collections;
using UnityEngine;

namespace OperaVR
{
    public class DragAndDropQuizPopup : APopup
    {
        [SerializeField]
        private DragAndDropContentController _dragAndDropContentController;

        public DragAndDropQuizPopupData DragAndDropData => Data as DragAndDropQuizPopupData;

        public override bool IsComplete => _dragAndDropContentController.IsCompleted();

        private Coroutine _completeCoroutine;

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
            if (_completeCoroutine != null)
            {
                return;
            }
            _completeCoroutine = StartCoroutine(DelayedComplete());
        }

        private IEnumerator DelayedComplete()
        {
            yield return new WaitForSeconds(1f);
            OnSuccess?.Invoke(this);
            PopupsManager.Instance.ClosePopup(this);
            _completeCoroutine = null;
        }
    }
}