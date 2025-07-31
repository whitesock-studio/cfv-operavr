using System.Collections.Generic;
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
            var isAnyCurrentQuest = _questSelectionPopupData.Quests.Any(q => q.Id == TESTQuestTracker.CurrentQuestId);
            for (var i = 0; i < _questsViews.Length; i++)
            {
                var view = _questsViews[i];
                if (i >= _questSelectionPopupData.Quests.Length)
                {
                    view.gameObject.SetActive(false);
                    continue;
                }
                var quest = _questSelectionPopupData.Quests[i];
                view.gameObject.SetActive(true);

                var targetState = QuestSelectionView.State.Normal;
                if (isAnyCurrentQuest)
                {
                    targetState = quest.Id == TESTQuestTracker.CurrentQuestId ?
                        QuestSelectionView.State.Selected : QuestSelectionView.State.NotInteractable;
                }
                if (TESTQuestTracker.IsQuestCompleted(quest.Id))
                {
                    targetState = QuestSelectionView.State.Completed;
                }

                view.SetState(targetState);
                view.LoadData(quest);
            }
        }

        private void OnQuestViewClicked(QuestSelectionView view)
        {
            TESTQuestTracker.CurrentQuestId = view.LoadedData.Id;
            RefreshData();
        }
    }
    
    public static class TESTQuestTracker
    {
        private static string _currentQuestId = "";
        public static string CurrentQuestId
        {
            get
            {
                if (IsQuestCompleted(_currentQuestId))
                {
                    _currentQuestId = "";
                }
                return _currentQuestId;
            }
            set => _currentQuestId = value;
        }

        public static List<string> CompletedQuestsIds = new();

        public static bool IsQuestCompleted(string questId)
        {
            return CompletedQuestsIds.Contains(questId);
        }

        public static bool TryCompleteQuest(string questId, out bool isAlreadyCompleted)
        {
            if (IsQuestCompleted(questId))
            {
                isAlreadyCompleted = true;
                return false;
            }

            if (CurrentQuestId == questId)
            {
                CurrentQuestId = "";
            }
            CompletedQuestsIds.Add(questId);
            isAlreadyCompleted = false;
            return true;
        }
    }
}
