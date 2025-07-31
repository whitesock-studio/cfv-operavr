using UnityEngine;

namespace OperaVR
{
    [CreateAssetMenu(menuName = "Popup/Data/Multitrack", fileName = "MultitrackPopupDataData_")]
    public class MultitrackPopupData : APopupData
    {
        public AudioClip[] Clips;
    }
}