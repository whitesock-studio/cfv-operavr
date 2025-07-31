using UnityEngine;

namespace OperaVR
{
    [CreateAssetMenu(menuName = "Popup/Data/Quest Selection", fileName = "QuestSelectionData_")]
    public class QuestSelectionPopupData : APopupData
    {
        public QuestData[] Quests;
    }

    [System.Serializable]
    public class QuestData
    {
        public string Id;
        public string DisplayedName;
    }
}