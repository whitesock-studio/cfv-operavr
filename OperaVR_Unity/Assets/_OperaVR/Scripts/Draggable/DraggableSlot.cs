using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OperaVR
{
    /// <summary>
    /// This is used by a DraggableManager as landing slots for draggables
    /// </summary>
    public class DraggableSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Action<DraggableSlot, PointerEventData> OnHoverEnterRequest;
        public Action<DraggableSlot, PointerEventData> OnHoverExitRequest;

        public Vector2 Position => transform.position;

        public bool IsHovered;

        public virtual void HoverStart(PointerEventData eventData)
        {
            IsHovered = true;
        }

        public virtual void HoverEnd(PointerEventData eventData)
        {
            IsHovered = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHoverEnterRequest?.Invoke(this, eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnHoverExitRequest?.Invoke(this, eventData);
        }
    }
}
