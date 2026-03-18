#if UNITY_EDITOR
using System.Collections.Generic;
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
            var usedIds = new List<string>();
            foreach (var stateTracker in stateTrackers)
            {
                if (!string.IsNullOrEmpty(stateTracker.Id))
                {
                    if (usedIds.Contains(stateTracker.Id))
                    {
                        stateTracker.Id = GUID.Generate().ToString();
                        EditorUtility.SetDirty(stateTracker);
                    }

                    usedIds.Add(stateTracker.Id);
                    continue;
                }

                stateTracker.Id = GUID.Generate().ToString();
                EditorUtility.SetDirty(stateTracker);
            }
        }
    }
}
#endif