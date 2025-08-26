using System;
using System.Collections.Generic;
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
        private ImageView _confirmView;

        [SerializeField]
        private ImageView _outcomeBorder;

        [SerializeField]
        private DraggableManager _draggableManager;

        private DragAndDropQuizPopupData _dragAndDropPopupData;

        private void Awake()
        {
            _confirmButton.onClick.AddListener(TryComplete);
            _draggableManager.OnPairingsChanged += OnPairingsChanged;
            _confirmView.SetInteractable(false, false);
        }

        public void LoadData(DragAndDropQuizPopupData data)
        {
            _dragAndDropPopupData = data;
            _questiontText.text = _dragAndDropPopupData.QuestionText;
            _trackPlayer.LoadClip(_dragAndDropPopupData.AudioClip);
            _draggableManager.LoadData(data.DraggableSlotsData, data.DraggablesData);
        }

        private void OnPairingsChanged(List<(int slotIndex, int draggableIndex)> pairings)
        {
            SetOutcomes(false, true);

            var areAllSlotsUsed = true;
            foreach (var pair in pairings)
            {
                if (pair.draggableIndex == -1)
                {
                    areAllSlotsUsed = false;
                    break;
                }
            }

            _confirmButton.interactable = areAllSlotsUsed;
            _confirmView.SetInteractable(areAllSlotsUsed, false);
        }

        public bool IsCompleted()
        {
            var pairings = _draggableManager.Pairings;
            if (pairings == null)
            {
                return false;
            }
            var isCorrect = true;
            foreach (var pair in pairings)
            {
                if (pair.draggableIndex == -1)
                {
                    isCorrect = false;
                    break;
                }
                var data = _dragAndDropPopupData.DraggablesData[pair.draggableIndex];
                if (pair.slotIndex != data.CorrectSlot)
                {
                    isCorrect = false;
                    break;
                }
            }
            return isCorrect;
        }

        private void TryComplete()
        {
            var isCorrect = IsCompleted();

            SetOutcomes(true, isCorrect);
            if (isCorrect)
            {
                Completed();
            }
            else
            {
                Fail();
            }
        }

        private void SetOutcomes(bool areGraphicsOn, bool isCorrect)
        {
            _confirmView.SetOutcome(areGraphicsOn, isCorrect);
            _draggableManager.SetOutcomes(areGraphicsOn, isCorrect);
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
