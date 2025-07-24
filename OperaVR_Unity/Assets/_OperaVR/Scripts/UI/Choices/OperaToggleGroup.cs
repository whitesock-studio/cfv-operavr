using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OperaVR
{
    public class OperaToggleGroup : MonoBehaviour
    {
        public bool AlwaysOneOn;

        public int MaxTogglesOn = 1;

        public bool SwitchAtMax = true;

        public bool SetNotInteractableOnMaxReached = false;

        [SerializeField]
        private List<OperaToggle> _toggles;
        public List<OperaToggle> Toggles => _toggles;

        public int TogglesOn => _toggles.Count(t => t.gameObject.activeInHierarchy && t.IsOn);
        public int TogglesOff => _toggles.Count(t => t.gameObject.activeInHierarchy && !t.IsOn);

        private void Awake()
        {
            foreach (var toggle in _toggles)
            {
                RegisterToToggleEvents(toggle);
            }
        }

        public void RegisterToggle(OperaToggle toggle)
        {
            if (_toggles.Contains(toggle))
            {
                return;
            }
            _toggles.Add(toggle);
            RegisterToToggleEvents(toggle);
        }

        public void DeregisterToggle(OperaToggle toggle)
        {
            if (!_toggles.Contains(toggle))
            {
                return;
            }
            _toggles.Remove(toggle);
            DeregisterFromToggleEvents(toggle);
        }

        private void RegisterToToggleEvents(OperaToggle toggle)
        {
            toggle.OnValueChanged += ToggleValueChanged;
        }

        private void DeregisterFromToggleEvents(OperaToggle toggle)
        {
            toggle.OnValueChanged -= ToggleValueChanged;
        }

        private void ToggleValueChanged(OperaToggle toggle, bool newValue)
        {
            var currentTogglesOn = TogglesOn;
            //Last has been set on
            if (newValue)
            {
                if (currentTogglesOn < MaxTogglesOn)
                {
                    return;
                }
                if (currentTogglesOn > MaxTogglesOn)
                {
                    if (!SwitchAtMax)
                    {
                        toggle.SetValue(false, false);
                        return;
                    } 
                    _toggles.ForEach(t =>
                    {
                        if(t != toggle && t.IsOn)
                        {
                            t.SetValue(false, false);
                            t.IsHidden = true;
                        }
                    });
                    return;
                }
                //Max just reached
                foreach (var t in _toggles)
                {
                    if (t.IsOn || !t.gameObject.activeInHierarchy)
                    {
                        continue;
                    }
                    if (!SetNotInteractableOnMaxReached)
                    {
                        continue;
                    }
                    if (SwitchAtMax)
                    {
                        t.IsHidden = true;
                    }
                    else
                    {
                        t.SetInteractable(false);
                    }
                }
                ;
                return;
            }

            //Last has been set off
            if (currentTogglesOn == MaxTogglesOn - 1)
            {
                foreach (var t in _toggles)
                {
                    if (t.IsOn || !t.gameObject.activeInHierarchy)
                    {
                        continue;
                    }
                    if (!SetNotInteractableOnMaxReached)
                    {
                        continue;
                    }
                    if (SwitchAtMax)
                    {
                        t.IsHidden = false;
                    }
                    else
                    {
                        t.SetInteractable(true);
                    }
                }
                return;
            }
            if (AlwaysOneOn && currentTogglesOn <= 0)
            {
                toggle.SetValue(true, false);
                return;
            }
            return;
        }
    }
}
