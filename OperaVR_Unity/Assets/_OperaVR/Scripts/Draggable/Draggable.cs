using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OperaVR
{
    /// <summary>
    /// This needs a DraggableManager over it to work properly as it sends requests and the Manager processes them
    /// </summary>
    public class Draggable : MonoBehaviour, 
        IPointerEnterHandler, 
        IPointerExitHandler, 
        IPointerDownHandler, 
        IPointerUpHandler, 
        IPointerMoveHandler
    {
        public Action<Draggable, PointerEventData> OnHoverEnterRequest;
        public Action<Draggable, PointerEventData> OnStartDragRequest;
        public Action<Draggable, PointerEventData> OnDragRequest;
        public Action<Draggable, PointerEventData> OnReleaseRequest;
        public Action<Draggable, PointerEventData> OnHoverExitRequest;

        public bool IsDragging;
        public bool IsHovered;

        public DraggableSlot StartingSlot;

        private Vector2 _startDragPosition;
        public Vector2 StartDragPosition => _startDragPosition;

        private void Start()
        {
            Reset();
        }

        public void Reset()
        {
            if (StartingSlot == null)
            {
                return;
            }
            StartingSlot.LinkedDraggable = this;
            transform.position = StartingSlot.Position;
        }

        public virtual void HoverStart(PointerEventData eventData)
        {
            IsHovered = true;
        }

        public virtual void StartDrag(PointerEventData eventData)
        {
            IsDragging = true;
            _startDragPosition = transform.position;
        }

        public virtual void Drag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public virtual void Release(PointerEventData eventData, Vector2 targetPosition)
        {
            IsDragging = false;
            transform.position = targetPosition;
        }

        public virtual void HoverEnd(PointerEventData eventData)
        {
            IsHovered = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHoverEnterRequest?.Invoke(this, eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnStartDragRequest?.Invoke(this, eventData);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            OnDragRequest?.Invoke(this, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnReleaseRequest?.Invoke(this, eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnHoverExitRequest?.Invoke(this, eventData);
        }
    }
}
