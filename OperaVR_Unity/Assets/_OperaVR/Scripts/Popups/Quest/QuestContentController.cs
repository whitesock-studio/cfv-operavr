using SpatialSys.UnitySDK;
using System.Linq;
using UnityEngine;

namespace OperaVR
{
    public class QuestContentController : MonoBehaviour
    {
        [SerializeField]
        private QuestSelectionView[] _questsViews;

        private QuestSelectionPopupData _questSelectionPopupData;

        private void Awake()
        {
            foreach (var questView in _questsViews)
            {
                questView.OnClicked += OnQuestViewClicked;
            }
        }

        public void LoadData(QuestSelectionPopupData data)
        {
            _questSelectionPopupData = data;

            RefreshData();
        }

        public void RefreshData()
        {
            var isAnyQuestInProgress = _questSelectionPopupData.Quests.Any(
                q => SpatialBridge.questService.quests[q.Id].status == QuestStatus.InProgress);
            for (var i = 0; i < _questsViews.Length; i++)
            {
                var view = _questsViews[i];
                if (i >= _questSelectionPopupData.Quests.Length)
                {
                    view.gameObject.SetActive(false);
                    continue;
                }
                var quest = SpatialBridge.questService.quests[_questSelectionPopupData.Quests[i].Id];
                view.gameObject.SetActive(true);

                var targetState = QuestSelectionView.State.Normal;
                if (isAnyQuestInProgress)
                {
                    targetState = quest.status == QuestStatus.InProgress ?
                        QuestSelectionView.State.Selected : QuestSelectionView.State.NotInteractable;
                }
                if (quest.status == QuestStatus.Completed)
                {
                    targetState = QuestSelectionView.State.Completed;
                }

                view.SetState(targetState);
                view.LoadData(quest);
            }
        }

        private void OnQuestViewClicked(QuestSelectionView view)
        {
            view.LoadedData.Start();
            RefreshData();
        }
    }
}
