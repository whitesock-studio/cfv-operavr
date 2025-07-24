using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperaVR
{
    public class DragAndDropContentController : MonoBehaviour
    {
        public Action OnComplete;
        public Action OnFail;

        [SerializeField]
        private TMP_Text _questiontText;

        [SerializeField]
        private TrackPlayer _trackPlayer;

        [SerializeField]
        private Button _confirmButton;

        [SerializeField]
        private ImageView _outcomeBorder;

        private DragAndDropQuizPopupData _dragAndDropPopupData;

        private void Awake()
        {
            _confirmButton.onClick.AddListener(TryComplete);
        }

        public void LoadData(DragAndDropQuizPopupData data)
        {
            _dragAndDropPopupData = data;
            _questiontText.text = _dragAndDropPopupData.QuestionText;
            _trackPlayer.LoadClip(_dragAndDropPopupData.AudioClip);
        }

        private void TryComplete()
        {

        }

        private void Fail()
        {
            OnFail?.Invoke();
        }

        private void Completed()
        {
            OnComplete?.Invoke();
        }
    }
}
