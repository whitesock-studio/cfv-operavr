using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OperaVR
{
    public class OperaToggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
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

        [SerializeField]
        private bool m_isInteractable = true;
        public virtual bool IsInteractable
        {
            get => m_isInteractable;
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

        [SerializeField]
        private bool _isHidden = true;
        public virtual bool IsHidden
        {
            get => _isHidden;
            set
            {
                if (_isHidden == value)
                {
                    return;
                }
                _isHidden = value;
                BackgroundView.SetHidden(_isHidden, IsOn);
                BorderView.SetHidden(_isHidden, IsOn);
                ToggleView?.SetHidden(_isHidden, IsOn);
            }
        }

        [SerializeField]
        private bool _isHovered;
        public virtual bool IsHovered
        {
            get => _isHovered;
            set
            {
                if (_isHovered == value)
                {
                    return;
                }
                _isHovered = value;
                BackgroundView.SetHovered(_isHovered, IsOn);
                BorderView.SetHovered(_isHovered, IsOn);
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

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsInteractable)
            {
                return;
            }
            BackgroundView.SetHeld(true, IsOn);
            BorderView.SetHeld(true, IsOn);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!IsInteractable)
            {
                return;
            }
            BackgroundView.SetHeld(false, IsOn);
            BorderView.SetHeld(false, IsOn);
            SetValue(!IsOn);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsInteractable)
            {
                return;
            }
            IsHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsInteractable)
            {
                return;
            }
            IsHovered = false;
        }
    }
}
