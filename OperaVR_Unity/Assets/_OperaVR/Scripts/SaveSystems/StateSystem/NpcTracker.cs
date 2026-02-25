using System.Collections;
using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    [RequireComponent(typeof(NPC))]
    public class NpcTracker : AStateTracker
    {
        private NPC _npc;
        
        protected override string GetVariableKey(string sceneKey) => base.GetVariableKey(sceneKey) + "_Npc";
        
        private void Awake()
        {
            _npc = GetComponent<NPC>();
        }

        public override void Save(string sceneKey)
        {
            StartCoroutine(SaveCor());
            return;

            IEnumerator SaveCor()
            {
                while (!_npc.HasAvatar)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(.1f); 
                var key = GetVariableKey(sceneKey);
                WorldData.SaveVariable(key, _npc.Avatar.position, _ => { });
            }
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
                while (!_npc.HasAvatar)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(.3f); 

                _npc.Avatar.position = request.vector3Value;
            }
        }
    }
}
