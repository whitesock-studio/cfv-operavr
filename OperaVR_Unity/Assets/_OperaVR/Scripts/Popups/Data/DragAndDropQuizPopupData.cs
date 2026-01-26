using UnityEngine;

namespace OperaVR
{
    [CreateAssetMenu(menuName = "Popup/Data/DragAndDropQuiz", fileName = "DragAndDropQuiz_")]
    public class DragAndDropQuizPopupData : APopupData
    {
        public string QuestionText;
        public AudioClip AudioClip;
        public Sprite Image;
        public DraggableSlotData[] DraggableSlotsData;
        public DraggableData[] DraggablesData;

        [System.Serializable]
        public class DraggableSlotData
        {
            public Vector2 Position;
        }

        [System.Serializable]
        public class DraggableData
        {
            public Sprite Sprite;

            [Tooltip("Set -1 if is not correct")]
            public int CorrectSlot = -1;
        }
    }
}