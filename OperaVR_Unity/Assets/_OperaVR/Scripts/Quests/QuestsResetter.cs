using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class QuestsResetter : MonoBehaviour
    {
        [SerializeField]
        private uint _questToResetID;
        
        public void ResetSingleQuest()
        {
            if (SpatialBridge.questService.quests.TryGetValue(_questToResetID, out var quest))
            {
                quest.Reset();
            }
        }

        public void ResetQuestInProgress()
        {
            foreach (var quest in SpatialBridge.questService.quests.Values)
            {
                if (quest.status == QuestStatus.InProgress)
                {
                    quest.Reset();
                }
            }
        }

        public void ResetCompletedQuests()
        {
            foreach (var quest in SpatialBridge.questService.quests.Values)
            {
                if (quest.status == QuestStatus.Completed)
                {
                    quest.Reset();
                }
            }
        }

        public void ResetAllQuests()
        {
            foreach (var quest in SpatialBridge.questService.quests.Values)
            {
                if (quest.status != QuestStatus.None)
                {
                    quest.Reset();
                }
            }
        }
    }
}
