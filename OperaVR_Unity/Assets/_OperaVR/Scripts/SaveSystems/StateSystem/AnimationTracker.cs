using System;
using System.Collections.Generic;
using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    [RequireComponent(typeof(Animator))]
    public class AnimationTracker : AStateTracker
    {
        private Animator _animator;
        
        protected override string GetVariableKey(string sceneKey) => base.GetVariableKey(sceneKey) + "_Anim";

        private Dictionary<string, object> _prevValues = new();
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _prevValues = new Dictionary<string, object>();
        }

        private void Update()
        {
            var sceneKey = StateSystem.Instance.Settings.SceneKey;
            foreach (var parameter in _animator.parameters)
            {
                var paramName = parameter.name;
                var key = GetVariableKey(sceneKey) + "_" + paramName;
                switch (parameter.type)
                {
                    case AnimatorControllerParameterType.Bool:
                        var boolValue = _animator.GetBool(paramName);
                        if (_prevValues.TryGetValue(key, out var currBool) && (bool)currBool != boolValue)
                        {
                            WorldData.SaveVariable(key, boolValue, _ => { });
                        }
                        if (!_prevValues.TryAdd(key, boolValue))
                        {
                            _prevValues[key] = boolValue;
                        }
                        break;
                    case AnimatorControllerParameterType.Float:
                        var floatValue = _animator.GetFloat(paramName);
                        if (_prevValues.TryGetValue(key, out var currFloat) && 
                            Math.Abs((float)currFloat - floatValue) > .01f)
                        {
                            WorldData.SaveVariable(key, floatValue, _ => { });
                        }
                        if (!_prevValues.TryAdd(key, floatValue))
                        {
                            _prevValues[key] = floatValue;
                        }
                        break;
                    case AnimatorControllerParameterType.Int:
                        var intValue = _animator.GetInteger(paramName);
                        if (_prevValues.TryGetValue(key, out var currInt) && (int)currInt != intValue)
                        {
                            WorldData.SaveVariable(key, intValue, _ => { });
                        }
                        if (!_prevValues.TryAdd(key, intValue))
                        {
                            _prevValues[key] = intValue;
                        }
                        break;
                    default:
                    case AnimatorControllerParameterType.Trigger:
                        break;
                }
            }
        }

        public override void Save(string sceneKey)
        {
            //done in Update
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
