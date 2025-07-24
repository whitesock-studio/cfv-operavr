using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OperaVR
{
    public class DraggableManager : MonoBehaviour
    {
        [SerializeField]
        private List<Draggable> _startingDraggables = new();
        private List<Draggable> _draggables = new();

        [SerializeField]
        private List<DraggableSlot> _startingDraggableSlots = new();
        private List<DraggableSlot> _draggableSlots = new();

        public Draggable CurrentDraggable;
        public DraggableSlot CurrentHoveredDraggableSlot;

        private void Awake()
        {
            foreach(var draggable in _startingDraggables)
            {
                RegisterDraggable(draggable);
            }

            foreach (var draggableSlot in _startingDraggableSlots)
            {
                RegisterDraggableSlot(draggableSlot);
            }
        }

        #region De/Registration Methods

        public void RegisterDraggable(Draggable draggable)
        {
            if (_draggables.Contains(draggable))
            {
                return;
            }
            _draggables.Add(draggable);
            draggable.OnHoverEnterRequest += DraggableHoverEnterRequested;
            draggable.OnStartDragRequest += DraggableStartDragRequested;
            draggable.OnDragRequest += DraggableDragRequested;
            draggable.OnReleaseRequest += DraggableReleaseRequested;
            draggable.OnHoverExitRequest += DraggableHoverExitRequested;
        }

        public void DeregisterDraggable(Draggable draggable)
        {
            if (!_draggables.Contains(draggable))
            {
                return;
            }
            _draggables.Remove(draggable);
            draggable.OnHoverEnterRequest -= DraggableHoverEnterRequested;
            draggable.OnStartDragRequest -= DraggableStartDragRequested;
            draggable.OnDragRequest -= DraggableDragRequested;
            draggable.OnReleaseRequest -= DraggableReleaseRequested;
            draggable.OnHoverExitRequest -= DraggableHoverExitRequested;
        }

        public void RegisterDraggableSlot(DraggableSlot draggableSlot)
        {
            if (_draggableSlots.Contains(draggableSlot))
            {
                return;
            }
            _draggableSlots.Add(draggableSlot);
            draggableSlot.OnHoverEnterRequest += DraggableSlotHoverEnterRequested;
            draggableSlot.OnHoverExitRequest += DraggableSlotHoverExitRequested;
        }

        public void DeregisterDraggableSlot(DraggableSlot draggableSlot)
        {
            if (!_draggableSlots.Contains(draggableSlot))
            {
                return;
            }
            _draggableSlots.Remove(draggableSlot);
            draggableSlot.OnHoverEnterRequest -= DraggableSlotHoverEnterRequested;
            draggableSlot.OnHoverExitRequest -= DraggableSlotHoverExitRequested;
        }
        
        #endregion

        #region Draggable Events Callbacks

        private void DraggableHoverEnterRequested(Draggable draggable, PointerEventData eventData)
        {
            if (CurrentDraggable != null)
            {
                return;
            }
            draggable.HoverStart(eventData);
        }

        private void DraggableStartDragRequested(Draggable draggable, PointerEventData eventData)
        {
            if (CurrentDraggable != null)
            {
                return;
            }
            CurrentDraggable = draggable;
            draggable.StartDrag(eventData);
        }

        private void DraggableDragRequested(Draggable draggable, PointerEventData eventData)
        {
            if (CurrentDraggable != draggable)
            {
                return;
            }
            draggable.Drag(eventData);
        }

        private void DraggableReleaseRequested(Draggable draggable, PointerEventData eventData)
        {
            if (CurrentDraggable != draggable)
            {
                return;
            }
            CurrentDraggable = null;

            var targetPosition = draggable.StartDragPosition;
            if (CurrentHoveredDraggableSlot != null)
            {
                targetPosition = CurrentHoveredDraggableSlot.Position;
            }
            
            draggable.Release(eventData, targetPosition);
        }
        
        private void DraggableHoverExitRequested(Draggable draggable, PointerEventData eventData)
        {
            if (CurrentDraggable != null)
            {
                return;
            }
            draggable.HoverEnd(eventData);
        }

        #endregion

        #region Draggable Slot Events Callbacks

        private void DraggableSlotHoverEnterRequested(DraggableSlot draggableSlot, PointerEventData eventData)
        {
            if (CurrentHoveredDraggableSlot != null)
            {
                return;
            }
            CurrentHoveredDraggableSlot = draggableSlot;
            draggableSlot.HoverStart(eventData);
        }

        private void DraggableSlotHoverExitRequested(DraggableSlot draggableSlot, PointerEventData eventData)
        {
            if (CurrentHoveredDraggableSlot == null)
            {
                return;
            }
            CurrentHoveredDraggableSlot = null;
            draggableSlot.HoverEnd(eventData);
        }

        #endregion
    }
}
