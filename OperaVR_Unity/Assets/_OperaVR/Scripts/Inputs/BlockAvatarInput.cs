using SpatialSys.UnitySDK;
using UnityEngine;

namespace OperaVR
{
    public class BlockAvatarInput : MonoBehaviour, IAvatarInputActionsListener
    {
        public void OnAvatarMoveInput(Vector2 move, InputPhase phase) { }
        public void OnAvatarLookInput(Vector2 delta, InputPhase phase) { }
        public void OnAvatarJumpInput(InputPhase phase) { }
        public void OnAvatarEmoteInput(string emoteId, InputPhase phase) { }
        public void OnAvatarMoveInput(InputPhase inputPhase, Vector2 inputMove) { }
        public void OnAvatarSprintInput(InputPhase inputPhase) { }
        public void OnAvatarActionInput(InputPhase inputPhase) { }
        public void OnAvatarAutoSprintToggled(bool on) { }
        public void OnInputCaptureStarted(InputCaptureType type) { }
        public void OnInputCaptureStopped(InputCaptureType type) { }
    }
}
