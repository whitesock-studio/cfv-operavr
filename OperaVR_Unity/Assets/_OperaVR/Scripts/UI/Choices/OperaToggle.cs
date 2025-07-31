using System;
using UnityEngine;

namespace OperaVR
{
    public class OperaToggle : UISelectableElement
    {
        public Action<OperaToggle, bool> OnValueChanged;
        public Action<OperaToggle, bool> OnInteractableChanged;

        [Header("State")]
        [SerializeField]
        private bool m_isOn;
        public virtual bool IsOn
        {
            get => m_isOn;
            protected set
            {
                if (m_isOn == value)
                {
                    return;
                }
                m_isOn = value;
                BackgroundView.SetSelected(m_isOn);
                BorderView.SetSelected(m_isOn);
                ToggleView?.SetOn(m_isOn);
            }
        }

        public override bool IsInteractable
        {
            get => base.IsInteractable;
            protected set
            {
                if (m_isInteractable == value)
                {
                    return;
                }
                m_isInteractable = value;
                BackgroundView.SetInteractable(m_isInteractable, IsOn);
                BorderView.SetInteractable(m_isInteractable, IsOn);
                ToggleView?.SetInteractable(m_isInteractable, IsOn);
            }
        }

        public override bool IsHidden
        {
            get => m_isHidden;
            set
            {
                if (m_isHidden == value)
                {
                    return;
                }
                m_isHidden = value;
                BackgroundView.SetHidden(m_isHidden, IsOn);
                BorderView.SetHidden(m_isHidden, IsOn);
                ToggleView?.SetHidden(m_isHidden, IsOn);
            }
        }

        public override bool IsHovered
        {
            get => m_isHovered;
            set
            {
                if (m_isHovered == value)
                {
                    return;
                }
                m_isHovered = value;
                BackgroundView.SetHovered(m_isHovered, IsOn);
                BorderView.SetHovered(m_isHovered, IsOn);
            }
        }

        [Header("Background and Border Views")]
        [SerializeField]
        protected ImageView BackgroundView;

        [SerializeField]
        protected ImageView BorderView;

        [Header("Toggle View")]
        [SerializeField]
        protected ToggleView ToggleView;

        public virtual void Reset()
        {
            SetValue(false);
            SetInteractable(true);
            IsHovered = false;
        }
        
        public void SetValue(bool value, bool notify = true)
        {
            IsOn = value;
            if (notify)
            {
                OnValueChanged?.Invoke(this, m_isOn);
            }
        }

        public void SetInteractable(bool value, bool notify = true)
        {
            if (value)
            {
                IsHidden = false;
            }
            IsInteractable = value;
            if (notify)
            {
                OnInteractableChanged?.Invoke(this, m_isInteractable);
            }
        }

        public void SetRawInteractable(bool value)
        {
            m_isInteractable = value;
        }

        protected override void OnPointerDown()
        {
            BackgroundView.SetHeld(true, IsOn);
            BorderView.SetHeld(true, IsOn);
        }

        protected override void OnPointerUp()
        {
            BackgroundView.SetHeld(false, IsOn);
            BorderView.SetHeld(false, IsOn);
            SetValue(!IsOn);
        }

        protected override void OnPointerEnter() { }

        protected override void OnPointerExit() { }
    }
}
