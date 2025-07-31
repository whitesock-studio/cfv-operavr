using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OperaVR
{
    public abstract class UISelectableElement : MonoBehaviour, 
        IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        protected bool m_isInteractable = true;
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
            }
        }

        [SerializeField]
        protected bool m_isHovered;
        public virtual bool IsHovered
        {
            get => m_isHovered;
            set
            {
                if (m_isHovered == value)
                {
                    return;
                }
                m_isHovered = value;
            }
        }

        [SerializeField]
        protected bool m_isHidden = true;
        public virtual bool IsHidden
        {
            get => m_isHidden;
            set
            {
                if (m_isHidden == value)
                {
                    return;
                }
                m_isHidden = value;
            }
        }

        protected abstract void OnPointerDown();
        protected abstract void OnPointerUp();
        protected abstract void OnPointerEnter();
        protected abstract void OnPointerExit();

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsInteractable)
            {
                return;
            }
            OnPointerDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!IsInteractable)
            {
                return;
            }
            OnPointerUp();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsInteractable)
            {
                return;
            }
            IsHovered = true;
            OnPointerEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsInteractable)
            {
                return;
            }
            IsHovered = false;
            OnPointerExit();
        }
    }
}
