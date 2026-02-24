#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace OperaVR
{
    [CustomEditor(typeof(StateSystem))]
    public class StateSystemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!GUILayout.Button("UpdateTrackers ID"))
            {
                return;
            }
            var stateTrackers = FindObjectsOfType<AStateTracker>(true);
            foreach (var stateTracker in stateTrackers)
            {
                if (!string.IsNullOrEmpty(stateTracker.Id))
                {
                    continue;
                }

                stateTracker.Id = GUID.Generate().ToString();
            }
        }
    }
}
#endif