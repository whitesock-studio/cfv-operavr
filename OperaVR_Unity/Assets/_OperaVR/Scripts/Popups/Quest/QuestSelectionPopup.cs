
using UnityEngine;

namespace OperaVR
{
    public class QuestSelectionPopup : APopup
    {
        [SerializeField]
        private QuestContentController _questContentController;

        public QuestSelectionPopupData QuestSelectionData => Data as QuestSelectionPopupData;

        [Header("TEST")]
        public string QuestId;
        public bool Completes;

        private void Update()
        {
            if (Completes)
            {
                TESTQuestTracker.TryCompleteQuest(QuestId, out var _);
            }
        }

        protected override void OnPreOpened()
        {
            base.OnPreOpened();
            _questContentController.LoadData(QuestSelectionData);
        }
    }
}