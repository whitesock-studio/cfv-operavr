using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static OperaVR.DragAndDropQuizPopupData;

namespace OperaVR
{
    public class DraggableManager : MonoBehaviour
    {
        public Action<List<(int slotIndex, int draggableIndex)>> OnPairingsChanged;

        public List<(int slotIndex, int draggableIndex)> Pairings;

        [SerializeField]
        private List<Draggable> _startingDraggables = new();
        private List<Draggable> _draggables = new();

        [SerializeField]
        private List<DraggableSlot> _startingInputDraggableSlots = new();
        private List<DraggableSlot> _inputDraggableSlots = new();

        [SerializeField]
        private List<DraggableSlot> _startingOutputDraggableSlots = new();
        private List<DraggableSlot> _outputDraggableSlots = new();

        public Draggable CurrentDraggable;
        public DraggableSlot CurrentHoveredDraggableSlot;
        public DraggableSlot PreviousDraggableSlot;

        private void Awake()
        {
            foreach(var draggable in _startingDraggables)
            {
                RegisterDraggable(draggable);
            }

            foreach (var draggableSlot in _startingInputDraggableSlots)
            {
                RegisterDraggableSlot(draggableSlot, true);
            }

            foreach (var draggableSlot in _startingOutputDraggableSlots)
            {
                RegisterDraggableSlot(draggableSlot, false);
            }
        }

        private void Update()
        {
            CheckHoveredSlot();
        }

        public void LoadData(DraggableSlotData[] draggableSlotsData, DraggableData[] draggablesData)
        {
            for (var i = 0; i < _outputDraggableSlots.Count; i++)
            {
                var slot = _outputDraggableSlots[i];
                slot.gameObject.SetActive(i < draggableSlotsData.Length);
                if (i < draggableSlotsData.Length)
                {
                    slot.transform.localPosition = draggableSlotsData[i].Position;
                    continue;
                }
            }

            for (var i = 0; i < _inputDraggableSlots.Count; i++)
            {
                var slot = _inputDraggableSlots[i];
                _inputDraggableSlots[i].gameObject.SetActive(i < draggablesData.Length);
            }

            for (var i = 0; i < _draggables.Count; i++)
            {
                var draggable = _draggables[i];
                draggable.gameObject.SetActive(i < draggablesData.Length);
                if (i < draggablesData.Length)
                {
                    draggable.transform.position = _inputDraggableSlots[i].Position;
                    continue;
                }
            }
        }

        public void SetOutcomes(bool areGraphicsOn, bool isCorrect)
        {
            foreach (var outputSlot in _outputDraggableSlots)
            {
                if (!outputSlot.gameObject.activeSelf)
                {
                    continue;
                }
                outputSlot.BorderView.SetOutcome(areGraphicsOn, isCorrect);
            }
        }

        private void CheckPairings()
        {
            Pairings = new List<(int, int)>();
            for (var i = 0; i < _outputDraggableSlots.Count; i++)
            {
                var slot = _outputDraggableSlots[i];
                if (!slot.gameObject.activeSelf)
                {
                    continue;
                }
                var linkedDraggableIndex = slot.LinkedDraggable == null ? -1 : 
                    _draggables.IndexOf(slot.LinkedDraggable);
                Pairings.Add((i, linkedDraggableIndex));
            }
            OnPairingsChanged?.Invoke(Pairings);
        }

        private void LinkDraggableToSlot(Draggable draggable, DraggableSlot slot)
        {
            draggable.transform.position = slot.Position;
            slot.LinkedDraggable = draggable;
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

        public void RegisterDraggableSlot(DraggableSlot draggableSlot, bool asInput)
        {
            var list = asInput ? _inputDraggableSlots : _outputDraggableSlots;
            if (list.Contains(draggableSlot))
            {
                return;
            }
            list.Add(draggableSlot);
        }

        public void DeregisterDraggableSlot(DraggableSlot draggableSlot, bool asInput)
        {
            var list = asInput ? _inputDraggableSlots : _outputDraggableSlots;
            if (!list.Contains(draggableSlot))
            {
                return;
            }
            list.Remove(draggableSlot);
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
            PreviousDraggableSlot = null;
            if (CurrentHoveredDraggableSlot != null && CurrentHoveredDraggableSlot.LinkedDraggable == draggable)
            {
                PreviousDraggableSlot = CurrentHoveredDraggableSlot;
                CurrentHoveredDraggableSlot.LinkedDraggable = null;
            }
            draggable.StartDrag(eventData);
            CheckPairings();
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

            if (CurrentHoveredDraggableSlot == null || 
                (!CurrentHoveredDraggableSlot.AcceptsDraggables && 
                draggable.StartingSlot != CurrentHoveredDraggableSlot))
            {
                if (PreviousDraggableSlot != null)
                {
                    LinkDraggableToSlot(draggable, PreviousDraggableSlot);
                    draggable.Release(eventData, PreviousDraggableSlot.Position);
                    CheckPairings();
                    return;
                }
                draggable.Release(eventData, draggable.StartDragPosition);
                CheckPairings();
                return;
            }

            if (CurrentHoveredDraggableSlot.IsOccupied)
            {
                var currentOccupyingDraggable = CurrentHoveredDraggableSlot.LinkedDraggable;
                if (currentOccupyingDraggable.StartingSlot != null)
                {
                    LinkDraggableToSlot(currentOccupyingDraggable, currentOccupyingDraggable.StartingSlot);
                }
            }

            LinkDraggableToSlot(draggable, CurrentHoveredDraggableSlot);
            draggable.Release(eventData, CurrentHoveredDraggableSlot.Position);
            CheckPairings();
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

        private void CheckHoveredSlot()
        {
            var mousePos = Input.mousePosition;
            var prevHoveredSlot = CurrentHoveredDraggableSlot;
            CurrentHoveredDraggableSlot = null;
            foreach (var slot in _inputDraggableSlots)
            {
                if (slot.IsPointInsideRect(mousePos))
                {
                    CurrentHoveredDraggableSlot = slot;
                    break;
                }
            }
            foreach (var slot in _outputDraggableSlots)
            {
                if (slot.IsPointInsideRect(mousePos))
                {
                    CurrentHoveredDraggableSlot = slot;
                    break;
                }
            }
            if (prevHoveredSlot == CurrentHoveredDraggableSlot)
            {
                return;
            }
            if (CurrentHoveredDraggableSlot != null && prevHoveredSlot == null)
            {
                CurrentHoveredDraggableSlot.IsHovered = true;
                return;
            }
            if (CurrentHoveredDraggableSlot == null && prevHoveredSlot != null)
            {
                prevHoveredSlot.IsHovered = false;
                return;
            }
        }
    }
}
