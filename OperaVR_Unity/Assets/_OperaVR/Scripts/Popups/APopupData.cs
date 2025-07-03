using UnityEngine;

namespace OperaVR
{
    public abstract class APopupData : ScriptableObject
    {
        public PopupType PopupType;

        public bool InterruptsMovement = true;
    }
}