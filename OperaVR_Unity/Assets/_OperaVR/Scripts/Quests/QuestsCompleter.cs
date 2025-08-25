using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class QuestsCompleter : MonoBehaviour
    {
        [SerializeField]
        private uint _questToCompleteID;
        
        public void CompleteSingleQuest()
        {
            if (SpatialBridge.questService.quests.TryGetValue(_questToCompleteID, out var quest))
            {
                quest.Complete();
            }
        }

        public void CompleteQuestInProgress()
        {
            foreach (var quest in SpatialBridge.questService.quests.Values)
            {
                if (quest.status == QuestStatus.InProgress)
                {
                    quest.Complete();
                }
            }
        }
    }
}
