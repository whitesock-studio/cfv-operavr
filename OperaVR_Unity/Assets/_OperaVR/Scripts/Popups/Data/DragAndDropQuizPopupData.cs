using UnityEngine;

namespace OperaVR
{
    [CreateAssetMenu(menuName = "Popup/Data/DragAndDropQuiz", fileName = "DragAndDropQuiz_")]
    public class DragAndDropQuizPopupData : APopupData
    {
        public string QuestionText;
        public AudioClip AudioClip;
        public GameObject DataPrefab;
        public DraggableData DraggablesData;

        [System.Serializable]
        public class DraggableData
        {
            public Sprite Sprite;
            public int CorrectSlot = -1;
        }
    }
}