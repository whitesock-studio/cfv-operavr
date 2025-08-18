using SpatialSys.UnitySDK;
using System;
using UnityEngine;

namespace OperaVR
{
    public class QuestSelectionView : UISelectableElement
    {
        public Action<QuestSelectionView> OnClicked;

        public enum State
        {
            Normal,
            NotInteractable,
            Selected,
            Completed
        }

        public ImageView BackgroundView;
        public ImageView BorderView;
        public TextView TextView;

        [SerializeField]
        private GameObject _completedIcon;

        private IQuest _loadedData;
        public IQuest LoadedData => _loadedData;

        private State _currentState;
        public State CurrentState => _currentState;

        private bool IsSelected => _currentState == State.Selected;

        public override bool IsHovered 
        { 
            get => base.IsHovered;
            set
            {
                base.IsHovered = value;
                if (_currentState != State.Normal)
                {
                    return;
                }
                BackgroundView.SetHovered(m_isHovered, IsSelected);
                BorderView.SetHovered(m_isHovered, IsSelected);
            }
        }

        public void LoadData(IQuest quest)
        {
            _loadedData = quest;
            TextView.TextDisplayer.text = _loadedData.name;
        }

        public void SetState(State state)
        {
            _currentState = state;
            _completedIcon.SetActive(state == State.Completed);
            IsInteractable = state == State.Normal;
            switch(state)
            {
                case State.Normal:
                    BackgroundView.SetSelected(false);
                    BorderView.SetSelected(false);
                    TextView.SetSelected(false);
                    break;
                case State.Selected:
                    BackgroundView.SetSelected(true);
                    BorderView.SetSelected(true);
                    TextView.SetSelected(true);
                    break;
                case State.NotInteractable:
                    BackgroundView.SetInteractable(false, IsSelected);
                    BorderView.SetInteractable(false, IsSelected);
                    TextView.SetInteractable(false);
                    break;
                case State.Completed:
                    BackgroundView.SetOutcome(true, true);
                    BorderView.SetOutcome(true, true);
                    TextView.SetCorrect(true, false);
                    break;
            }
        }

        protected override void OnPointerDown()
        {
            BackgroundView.SetHeld(true, IsSelected);
            BorderView.SetHeld(true, IsSelected);
        }

        protected override void OnPointerUp()
        {
            BackgroundView.SetHeld(false, IsSelected);
            BorderView.SetHeld(false, IsSelected);
            OnClicked?.Invoke(this);
        }

        protected override void OnPointerEnter() { }

        protected override void OnPointerExit() { }
    }
}
