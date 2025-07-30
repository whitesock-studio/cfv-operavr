using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OperaVR
{
    /// <summary>
    /// This is used by a DraggableManager as landing slots for draggables
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class DraggableSlot : MonoBehaviour
    {
        public Vector2 Position => transform.position;

        public bool IsHovered;
        
        [NonSerialized]
        public Draggable LinkedDraggable;

        public bool IsOccupied => LinkedDraggable != null;

        public bool AcceptsDraggables = true;

        public ImageView ImageView;
        public ImageView BorderView;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (LinkedDraggable == null)
            {
                return;
            }
            LinkedDraggable.transform.position = Position;
        }

        public virtual void HoverStart(PointerEventData eventData)
        {
            IsHovered = true;
        }

        public virtual void HoverEnd(PointerEventData eventData)
        {
            IsHovered = false;
        }

        public bool IsPointInsideRect(Vector2 screenPoint)
        {
            if (!isActiveAndEnabled)
            {
                return false;
            }
            var localPoint = screenPoint - Position;
            return _rectTransform.rect.Contains(localPoint);
        }
    }
}
