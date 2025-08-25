using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class QuestSelectionPopup : APopup
    {
        [SerializeField]
        private QuestContentController _questContentController;

        public QuestSelectionPopupData QuestSelectionData => Data as QuestSelectionPopupData;

        public override bool IsComplete => false;

        [Header("TEST")]
        public uint QuestId;
        public bool Completes;

        private void Update()
        {
            if (Completes)
            {
                SpatialBridge.questService.quests[QuestId].Complete();
                Completes = false;
            }
        }

        protected override void OnPreOpened()
        {
            base.OnPreOpened();
            _questContentController.LoadData(QuestSelectionData);
        }
    }
}