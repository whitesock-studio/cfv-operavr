using System;
using System.Collections.Generic;
using System.Linq;
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
        protected List<Draggable> StartingDraggables = new();
        protected List<Draggable> Draggables = new();

        [SerializeField]
        protected List<DraggableSlot> StartingInputDraggableFewSlots = new();
        [SerializeField]
        protected List<DraggableSlot> StartingInputDraggableManySlots = new();
        protected List<DraggableSlot> InputDraggableSlots = new();

        [SerializeField]
        protected List<DraggableSlot> StartingOutputDraggableSlots = new();
        protected List<DraggableSlot> OutputDraggableSlots = new();

        public Draggable CurrentDraggable;
        public DraggableSlot CurrentHoveredDraggableSlot;
        public DraggableSlot PreviousDraggableSlot;

        protected virtual void Awake()
        {
            foreach(var draggable in StartingDraggables)
            {
                RegisterDraggable(draggable);
            }

            foreach (var draggableSlot in StartingOutputDraggableSlots)
            {
                RegisterDraggableSlot(draggableSlot, false);
            }
        }

        protected virtual void Update()
        {
            CheckHoveredSlot();
        }

        public void Reset()
        {
            for (var i = 0; i < OutputDraggableSlots.Count; i++)
            {
                var slot = OutputDraggableSlots[i];
                slot.LinkedDraggable = null;
            }

            for (var i = 0; i < InputDraggableSlots.Count; i++)
            {
                var slot = InputDraggableSlots[i];
                slot.LinkedDraggable = null;
            }

            for (var i = 0; i < Draggables.Count; i++)
            {
                var draggable = Draggables[i];
                draggable.Reset();
            }

            SetOutcomes(false, false);
        }

        public virtual void LoadData(DraggableSlotData[] draggableSlotsData, DraggableData[] draggablesData)
        {
            Reset();
            for (var i = 0; i < OutputDraggableSlots.Count; i++)
            {
                var slot = OutputDraggableSlots[i];
                slot.gameObject.SetActive(i < draggableSlotsData.Length);
                if (i >= draggableSlotsData.Length)
                {
                    continue;
                }
                slot.transform.localPosition = draggableSlotsData[i].Position;
                slot.transform.localScale = draggableSlotsData[i].Scale;
            }

            if (draggablesData.Length <= StartingInputDraggableFewSlots.Count)
            {
                foreach (var draggableSlot in StartingInputDraggableFewSlots)
                {
                    RegisterDraggableSlot(draggableSlot, true);
                }
                foreach (var draggableSlot in StartingInputDraggableManySlots)
                {
                    DeregisterDraggableSlot(draggableSlot, true);
                }
            }
            else
            {
                foreach (var draggableSlot in StartingInputDraggableManySlots)
                {
                    RegisterDraggableSlot(draggableSlot, true);
                }
                foreach (var draggableSlot in StartingInputDraggableFewSlots)
                {
                    DeregisterDraggableSlot(draggableSlot, true);
                }
            }
            
            for (var i = 0; i < InputDraggableSlots.Count; i++)
            {
                var slot = InputDraggableSlots[i];
                slot.gameObject.SetActive(i < draggablesData.Length);
            }

            for (var i = 0; i < Draggables.Count; i++)
            {
                var draggable = Draggables[i];
                draggable.gameObject.SetActive(i < draggablesData.Length);
                if (i >= draggablesData.Length)
                {
                    continue;
                }

                draggable.StartingSlot = InputDraggableSlots[i];
                draggable.transform.position = InputDraggableSlots[i].Position;
            }
        }

        public virtual void SetOutcomes(bool areGraphicsOn, bool isCorrect)
        {
            foreach (var outputSlot in OutputDraggableSlots.Where(
                         outputSlot => outputSlot.gameObject.activeSelf))
            {
                outputSlot.BorderView.SetOutcome(areGraphicsOn, isCorrect);
            }
        }

        protected virtual void CheckPairings()
        {
            Pairings = new List<(int, int)>();
            for (var i = 0; i < OutputDraggableSlots.Count; i++)
            {
                var slot = OutputDraggableSlots[i];
                if (!slot.gameObject.activeSelf)
                {
                    continue;
                }
                var linkedDraggableIndex = slot.LinkedDraggable == null ? -1 : 
                    Draggables.IndexOf(slot.LinkedDraggable);
                Pairings.Add((i, linkedDraggableIndex));
            }
            OnPairingsChanged?.Invoke(Pairings);
        }

        private void LinkDraggableToSlot(Draggable draggable, DraggableSlot slot)
        {
            slot.LinkedDraggable = draggable;
        }

        #region De/Registration Methods

        public void RegisterDraggable(Draggable draggable)
        {
            if (Draggables.Contains(draggable))
            {
                return;
            }
            Draggables.Add(draggable);
            draggable.OnHoverEnterRequest += DraggableHoverEnterRequested;
            draggable.OnStartDragRequest += DraggableStartDragRequested;
            draggable.OnDragRequest += DraggableDragRequested;
            draggable.OnReleaseRequest += DraggableReleaseRequested;
            draggable.OnHoverExitRequest += DraggableHoverExitRequested;
        }

        public void DeregisterDraggable(Draggable draggable)
        {
            if (!Draggables.Contains(draggable))
            {
                return;
            }
            Draggables.Remove(draggable);
            draggable.OnHoverEnterRequest -= DraggableHoverEnterRequested;
            draggable.OnStartDragRequest -= DraggableStartDragRequested;
            draggable.OnDragRequest -= DraggableDragRequested;
            draggable.OnReleaseRequest -= DraggableReleaseRequested;
            draggable.OnHoverExitRequest -= DraggableHoverExitRequested;
        }

        public void RegisterDraggableSlot(DraggableSlot draggableSlot, bool asInput)
        {
            var list = asInput ? InputDraggableSlots : OutputDraggableSlots;
            if (list.Contains(draggableSlot))
            {
                return;
            }
            list.Add(draggableSlot);
        }

        public void DeregisterDraggableSlot(DraggableSlot draggableSlot, bool asInput)
        {
            var list = asInput ? InputDraggableSlots : OutputDraggableSlots;
            draggableSlot.gameObject.SetActive(false);
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
            CurrentDraggable.transform.SetAsLastSibling();
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
            foreach (var slot in InputDraggableSlots)
            {
                if (slot.IsPointInsideRect(mousePos))
                {
                    CurrentHoveredDraggableSlot = slot;
                    break;
                }
            }
            foreach (var slot in OutputDraggableSlots)
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
