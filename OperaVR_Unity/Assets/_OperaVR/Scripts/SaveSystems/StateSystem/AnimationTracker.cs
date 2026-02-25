using System;
using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    [RequireComponent(typeof(Animator))]
    public class AnimationTracker : AStateTracker
    {
        private Animator _animator;
        
        protected override string GetVariableKey(string sceneKey) => base.GetVariableKey(sceneKey) + "_Anim";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public override void Save(string sceneKey)
        {
            foreach (var parameter in _animator.parameters)
            {
                var paramName = parameter.name;
                var key = GetVariableKey(sceneKey) + "_" + paramName;
                switch (parameter.type)
                {
                    case AnimatorControllerParameterType.Bool:
                        WorldData.SaveVariable(key, _animator.GetBool(paramName), _ => { });
                        break;
                    case AnimatorControllerParameterType.Float:
                        WorldData.SaveVariable(key, _animator.GetFloat(paramName), _ => { });
                        break;
                    case AnimatorControllerParameterType.Int:
                        WorldData.SaveVariable(key, _animator.GetInteger(paramName), _ => { });
                        break;
                    default:
                    case AnimatorControllerParameterType.Trigger:
                        break;
                }
            }
        }

        public override void Load(string sceneKey)
        {
            foreach (var parameter in _animator.parameters)
            {
                var paramName = parameter.name;
                var key = GetVariableKey(sceneKey) + "_" + paramName;
                WorldData.HasVariable(key, 
                    c => HasVariableCallback(c, parameter.type, key, paramName));
            }
            return;
            
            void HasVariableCallback(
                DataStoreHasVariableRequest request, AnimatorControllerParameterType type, string key, string paramName)
            {
                if (!request.hasVariable)
                {
                    return;
                }
                WorldData.TryGetVariable(key, 
                    c => GetVariableCallback(c, type, paramName));
            }

            void GetVariableCallback(
                DataStoreGetVariableRequest request, AnimatorControllerParameterType type, string paramName)
            {
                switch (type)
                {
                    case AnimatorControllerParameterType.Float:
                        _animator.SetFloat(paramName, request.floatValue);
                        break;
                    case AnimatorControllerParameterType.Int:
                        _animator.SetInteger(paramName, request.intValue);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        _animator.SetBool(paramName, request.boolValue);
                        break;
                    default:
                    case AnimatorControllerParameterType.Trigger:
                        break;
                }
            }
        }
    }
}
