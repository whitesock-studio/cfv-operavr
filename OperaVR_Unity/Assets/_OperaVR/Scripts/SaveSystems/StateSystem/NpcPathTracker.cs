using System;
using System.Collections;
using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class NpcPathTracker : AStateTracker
    {
        private enum Status
        {
            NotStarted,
            Started,
            Ended
        }
        
        private NpcPathStarter _npcPathStarter;
        private Status _state;

        private bool _isDirty;
        public override bool IsDirty => _isDirty;
        protected override string GetVariableKey(string sceneKey) => base.GetVariableKey(sceneKey) + "_NpcPath";
        
        private void Awake()
        {
            _npcPathStarter = GetComponent<NpcPathStarter>();
            if (!_npcPathStarter)
            {
                enabled = false;
                return;
            }
            _npcPathStarter.OnStarted.AddListener(() => StartCoroutine(Started()));
            _npcPathStarter.OnComplete.AddListener(() => StartCoroutine(Completed()));
            _isDirty = true;
        }

        private IEnumerator Started()
        {
            yield return null;
            _state = Status.Started;
            _isDirty = true;
        }
        
        private IEnumerator Completed()
        {
            yield return new WaitForSeconds(5f);
            _state = Status.Ended;
            _isDirty = true;
        }

        public override void Save(string sceneKey)
        {
            _isDirty = false;
            if (!enabled)
            {
                return;
            }
            var key = GetVariableKey(sceneKey);
            WorldData.SaveVariable(key, (int)_state, _ => { });
        }

        public override void Load(string sceneKey)
        {
            if (!enabled)
            {
                return;
            }
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

                _state = (Status)request.intValue;
                switch (_state)
                {
                    case Status.NotStarted:
                    case Status.Ended:
                    default:
                        break;
                    case Status.Started:
                        _npcPathStarter.Activate();
                        break;
                }
            }
        }
    }
}
