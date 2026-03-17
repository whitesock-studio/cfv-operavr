using System.Collections;
using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    [RequireComponent(typeof(NPCCharacter))]
    public class NpcTracker : AStateTracker
    {
        private NPCCharacter _npc;
        private Vector3 _lastSavedPosition;

        private bool _isDirty;
        public override bool IsDirty => _isDirty;
        protected override string GetVariableKey(string sceneKey) => base.GetVariableKey(sceneKey) + "_Npc";
        
        private void Awake()
        {
            _npc = GetComponent<NPCCharacter>();
            _lastSavedPosition = _npc.transform.position;
        }

        private void Update()
        {
            if (Vector3.Distance(_npc.transform.position, _lastSavedPosition) < .5f)
            {
                return;
            } 
            _isDirty = true;
        }

        public override void Save(string sceneKey)
        {
            _isDirty = false;
            _lastSavedPosition = _npc.transform.position;
            var key = GetVariableKey(sceneKey);
            WorldData.SaveVariable(key, _npc.transform.position, _ => { });
        }

        public override void Load(string sceneKey)
        {
            var key = GetVariableKey(sceneKey);
            WorldData.HasVariable(key, c => HasVariableCallback(c, key));
            return;
            
            void HasVariableCallback(DataStoreHasVariableRequest request, string key)
            {
                if (!request.hasVariable)
                {
                    return;
                }
                WorldData.TryGetVariable(key, 
                    c => StartCoroutine(GetVariableCallback(c)));
            }

            IEnumerator GetVariableCallback(DataStoreGetVariableRequest request)
            {
                yield return new WaitForSeconds(.3f); 

                _npc.transform.position = request.vector3Value;
                _npc.SetDestination(request.vector3Value);
                _npc.Stop();
            }
        }
    }
}
