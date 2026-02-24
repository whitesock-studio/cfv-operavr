using UnityEngine;

namespace OperaVR
{
    [CreateAssetMenu(menuName = "OPERA/Save System settings", fileName = "SaveSystemsSettings_Scene")]
    public class SaveSystemsSettings : ScriptableObject
    {
        public string SceneKey = "SceneKey";
        public float TimeBetweenSaves = 3f;
        public float StartDelay = 10f;
    }
}
