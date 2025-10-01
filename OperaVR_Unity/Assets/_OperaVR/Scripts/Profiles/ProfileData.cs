using UnityEngine;

namespace OperaVR
{
    [CreateAssetMenu(menuName = "Profiling/Profile Data", fileName = "Profile_")]
    public class ProfileData : ScriptableObject
    {
        [Header("Image")]
        public Sprite Image;

        [Header("Title")]
        public string Title;
        public string TitleKey;

        [Header("Score")]
        public string ScoreDescription;
        public string ScoreDescriptionKey;

        [Header("Description")]
        public string Description;
        public string DescriptionKey;
    }
}
